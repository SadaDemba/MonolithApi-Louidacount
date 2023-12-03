using Microsoft.CodeAnalysis;
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
    public class ProductReductionService : IProductReductionService
    {
        private readonly AppDatabaseContext _context;

        public ProductReductionService(AppDatabaseContext context)
        {
            _context = context;
        }

        public async Task Delete(int id, string userId)
        {
            ProductReduction? prodReduc = await _context.ProductReductions.
                Include(pr => pr.Product).ThenInclude(p => p!.Shop).
                FirstOrDefaultAsync(pr => pr.Id == id);
            if (prodReduc is null) throw new KeyNotFoundException(Constants.PRODUCT_REDUCTION_NOT_FOUND);

            if (prodReduc.Product!.Shop!.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            if (prodReduc.IsActivated == true) throw new BadHttpRequestException(Constants.DEPENDENCY_ERROR_BIS);

            _context.ProductReductions.Remove(prodReduc);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductReduction> Get(int id)
        {
            ProductReduction? prodReduc = await _context.ProductReductions.Include(pr => pr.Product).Include(pr => pr.Reduction).FirstOrDefaultAsync(pr => pr.Id == id);

            if (prodReduc is null) throw new KeyNotFoundException(Constants.PRODUCT_REDUCTION_NOT_FOUND);

            return prodReduc;
        }

        public async Task<IEnumerable<ProductReduction>> GetAll()
        {
            return await _context.ProductReductions.AsNoTracking().
                Include(pr => pr.Product).Include(pr => pr.Reduction).
                OrderBy(r => r.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<ProductReduction>> GetAllByShop(int shopId)
        {
            return await _context.ProductReductions.AsNoTracking().
                Where(pr => pr.Product!.ShopId == shopId).
                Include(pr => pr.Product).
                Include(pr => pr.Reduction).
                OrderBy(r => r.CreatedAt).ToListAsync();
        }

        public async Task<ResponseResource<ProductReduction>> GetAllPaginatedByShop(int shopId, string pageNumber, string pageSize)
        {
            IQueryable<ProductReduction> source = _context.ProductReductions.AsNoTracking().
                Where(pr => pr.Product!.ShopId == shopId).
                Include(pr => pr.Product).Include(pr => pr.Reduction).
                OrderBy(pr => pr.CreatedAt);

            return await PaginationService<ProductReduction>.Paginate(pageNumber,pageSize,source);
        }

        public async Task<ResponseResource<ProductReduction>> GetAllPaginated(string pageNumber, string pageSize)
        {
            IQueryable<ProductReduction> source = _context.ProductReductions.AsNoTracking().
                Include(pr => pr.Product).
                Include(pr => pr.Reduction).
                OrderBy(pr => pr.CreatedAt);

            return await PaginationService<ProductReduction>.Paginate(pageNumber, pageSize, source);
        }

        public async Task<ResponseResource<ProductReduction>> GetByProductAndShop(int shopId, int productId, string pageNumber, string pageSize)
        {
            IQueryable<ProductReduction> source = _context.ProductReductions.AsNoTracking().
                Where(pr => pr.ProductId == productId).
                Where(pr => pr.Product!.ShopId == shopId).
                Include(pr => pr.Product).Include(pr => pr.Reduction).
                OrderBy(pr => pr.CreatedAt);

            return await PaginationService<ProductReduction>.Paginate(pageNumber, pageSize, source);
        }

        public async Task<ResponseResource<ProductReduction>> GetByProduct(int productId, string pageNumber, string pageSize)
        {
            IQueryable<ProductReduction> source = _context.ProductReductions.AsNoTracking().
                Where(pr => pr.ProductId == productId).
                Include(pr => pr.Product).Include(pr => pr.Reduction).
                OrderBy(pr => pr.CreatedAt);

            return await PaginationService<ProductReduction>.Paginate(pageNumber, pageSize, source);
        }

        public async Task<ResponseResource<ProductReduction>> GetByReduction(int shopId, int reductionId, string pageNumber, string pageSize)
        {
            IQueryable<ProductReduction> source = _context.ProductReductions.AsNoTracking().
                Where(pr => pr.ReductionId == reductionId).
                Where(pr => pr.Product!.ShopId == shopId).
                Include(pr => pr.Product).Include(pr => pr.Reduction).
                OrderBy(pr => pr.CreatedAt);

            return await PaginationService<ProductReduction>.Paginate(pageNumber, pageSize, source);
        }

        public async Task<ResponseResource<ProductReduction>> GetSwitchIsActivatedByShop(int shopId, bool isActivted, string pageNumber, string pageSize)
        {
            IQueryable<ProductReduction> source = _context.ProductReductions.AsNoTracking().
                Where(pr => pr.Product!.ShopId == shopId).
                Where(pr =>pr.IsActivated == isActivted).
                Include(pr => pr.Product).Include(pr => pr.Reduction).
                OrderBy(pr => pr.CreatedAt);

            return await PaginationService<ProductReduction>.Paginate(pageNumber, pageSize, source);
        }

        public async Task<ResponseResource<ProductReduction>> GetSwitchIsActivated(bool isActivted, string pageNumber, string pageSize)
        {
            IQueryable<ProductReduction> source = _context.ProductReductions.AsNoTracking().
                Where(pr => pr.IsActivated == isActivted).
                Include(pr => pr.Product).Include(pr => pr.Reduction).
                OrderBy(pr => pr.CreatedAt);

            return await PaginationService<ProductReduction>.Paginate(pageNumber, pageSize, source);
        }

        public async Task<ProductReduction> SetIsActivated(int id, string userId, bool activate)
        {
            ProductReduction? prodReduct = _context.ProductReductions.
               Include(pr => pr.Reduction).
               Include(pr => pr.Product).ThenInclude(p => p!.Shop).
               FirstOrDefault(pr => pr.Id == id);

            if (prodReduct is null) throw new KeyNotFoundException(Constants.PRODUCT_REDUCTION_NOT_FOUND);

            if (prodReduct.Product!.Shop!.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            if (prodReduct.IsActivated == activate) throw new KeyNotFoundException(Constants.CURRENT_STATE);
            
            prodReduct.IsActivated = activate;
            prodReduct.UpdatedAt = DateTime.UtcNow;

            _context.Update(prodReduct);

            await _context.SaveChangesAsync();

            return prodReduct;
        }

        public async Task<ProductReduction> Post(string userId, ProductReduction prodReduct)
        {
            //Check if product exists
            Product? product = _context.Products.Include(p=>p.Shop).FirstOrDefault(p => p.ProductId ==  prodReduct.ProductId);
            if (product is null) throw new KeyNotFoundException(Constants.PRODUCT_NOT_FOUND);

            if (product.Shop!.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            //Check if Reduction exists
            if (!_context.Reductions.Any(r => r.Id == prodReduct.ReductionId)) throw new KeyNotFoundException(Constants.REDUCTION_NOT_FOUND);
            _context.ProductReductions.Add(prodReduct);

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
            return prodReduct;
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<ProductReduction>> GetMostUsed()
        {
            return await _context.ProductReductions.AsNoTracking().
                Where(pr => pr.IsActivated).
                Include(pr=> pr.Reduction).
               OrderByDescending(pr => pr.Reduction!.Percentage).Take(5).ToListAsync();
        }

        public async Task<ProductReduction> Put(int id, string userId, ProductReduction prodReduct)
        {
            if (prodReduct.Id != id) throw new BadHttpRequestException(Constants.INVALID_ID);

            ProductReduction? prodReduct1 = _context.ProductReductions.
                Include(pr => pr.Reduction).
                Include(pr => pr.Product).ThenInclude(p=>p!.Shop).
                FirstOrDefault(pr => pr.Id == id);

            if(prodReduct1 is null) throw new KeyNotFoundException(Constants.PRODUCT_REDUCTION_NOT_FOUND);

            if (prodReduct1.Product!.Shop!.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            //Check if product exists
            if (!_context.Products.Any(p => p.ProductId == prodReduct.ProductId)) throw new KeyNotFoundException(Constants.PRODUCT_NOT_FOUND);

            //Check if Reduction exists
            if (!_context.Reductions.Any(r => r.Id == prodReduct.ReductionId)) throw new KeyNotFoundException(Constants.REDUCTION_NOT_FOUND);

            prodReduct.UpdatedAt = DateTime.UtcNow;
            _context.Entry(prodReduct).State = EntityState.Modified;
            _context.Entry(prodReduct).Property(a => a.CreatedAt).IsModified = false;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException is PostgresException sql)
                {
                    HandleException(sql);
                }
                throw;
            }
            return prodReduct;
        }

        /// <summary>
        /// Catch the exception for unique field
        /// </summary>
        /// <param name="exception"></param>
        /// <exception cref="BadHttpRequestException"></exception>
        private static void HandleException(PostgresException exception)
        {
            if (exception.ConstraintName!.Equals("IX_ProductReductions_ProductId_ReductionId"))
                throw new BadHttpRequestException(Constants.REDUCTION_EXIST);
        }
    }
}
