using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MonolithApi.Context;
using MonolithApi.Interfaces;
using MonolithApi.Models;
using MonolithApi.Resources.Pagination;
using MonolithApi.Services.Pagination;
using MonolithApi.Utils;
using Npgsql;

namespace MonolithApi.Services
{
    public class ReductionService : IReductionService
    {
        private readonly AppDatabaseContext _context;
        public ReductionService(AppDatabaseContext context) {
            _context = context;
        }

        /// <inheritdoc/>
        /// <exception cref="KeyNotFoundException">Supression of a row in use</exception>
        public async Task Delete(int id, string userId)
        {
            Reduction? reduction = await _context.Reductions.AsNoTracking().Include(r => r.Shop).FirstOrDefaultAsync(r => r.Id == id);

            if (reduction is null) throw new KeyNotFoundException(Constants.REDUCTION_NOT_FOUND);

            if (reduction.Shop!.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            if (_context.ProductReductions.Any(pr => pr.ReductionId == id))
                throw new KeyNotFoundException(Constants.DEPENDENCY_ERROR);

            _context.Reductions.Remove(reduction);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Reduction> Get(int id)
        {
            Reduction? reduction = await _context.Reductions.Include(r => r.Shop).FirstOrDefaultAsync(r => r.Id == id);

            if (reduction is null) throw new KeyNotFoundException(Constants.REDUCTION_NOT_FOUND);

            return reduction;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Reduction>> GetAll()
        {
            return await _context.Reductions.AsNoTracking().
                Include(r=> r.Shop).
                Include(r => r.ProductReductions!).ThenInclude(pr=> pr.Product).
                OrderBy(r => r.ShopId).ToListAsync();  
        }

        /// <inheritdoc/>
        public async Task<ResponseResource<Reduction>> GetAllPaginated(string pageNumber, string pageSize)
        {
            IQueryable<Reduction> source = _context.Reductions.AsNoTracking().
                Include(r => r.Shop).
                Include(r => r.ProductReductions!).ThenInclude(pr => pr.Product).
                OrderBy(r => r.ShopId);

            return await PaginationService<Reduction>.Paginate(pageNumber, pageSize, source);
        }

        /// <inheritdoc/>
        public async Task<ResponseResource<Reduction>> GetAllPaginatedByShop(int shopId, string userId, string pageNumber, string pageSize)
        {
            Shop? shop = _context.Shops.Find(shopId);

            if(shop is null) throw new KeyNotFoundException(Constants.SHOP_NOT_FOUND);

            if (shop.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            IQueryable<Reduction> source = _context.Reductions.AsNoTracking().
                Where(r => r.ShopId == shopId).
                Include(r => r.Shop).
                Include(r => r.ProductReductions!).ThenInclude(pr => pr.Product).
                OrderBy(r => r.ShopId);

            return await PaginationService<Reduction>.Paginate(pageNumber, pageSize, source);
        }

        /// <inheritdoc/>
        public async Task<Reduction> Post(string userId, Reduction reduction)
        {
            Shop? shop = _context.Shops.Find(reduction.ShopId);

            if (shop is null) throw new KeyNotFoundException(Constants.SHOP_NOT_FOUND);

            if (shop.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            _context.Reductions.Add(reduction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException is PostgresException postgres)
                {
                    HandleException(postgres);
                }
                throw;
            }
            return reduction;
        }

        /// <inheritdoc/>
        public async Task<Reduction> Put(int id, string userId, Reduction reduction)
        {
            if (reduction.Id != id) throw new BadHttpRequestException(Constants.INVALID_ID);

            Reduction? reduction1 = _context.Reductions.AsNoTracking().Include(r => r.Shop).FirstOrDefault(r => r.Id == id);

            if (reduction1 is null) throw new KeyNotFoundException(Constants.REDUCTION_NOT_FOUND);

            if (reduction1.Shop!.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            reduction.UpdatedAt = DateTime.UtcNow;
            _context.Entry(reduction).State = EntityState.Modified;
            _context.Entry(reduction).Property(a => a.CreatedAt).IsModified = false;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException is PostgresException postgres)
                {
                    HandleException(postgres);
                }
                throw;
            }
            return reduction;
        }

        /// <summary>
        /// Catch the exception for unique field
        /// </summary>
        /// <param name="exception"></param>
        /// <exception cref="BadHttpRequestException"></exception>
        private static void HandleException(PostgresException exception)
        {
            if (exception.ConstraintName!.Equals("IX_Reductions_Title_ShopId"))
                throw new BadHttpRequestException(Constants.REDUCTION_EXIST);
        }
    }
}
