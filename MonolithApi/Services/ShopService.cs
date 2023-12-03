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
    public class ShopService : IShopService
    {
        private readonly AppDatabaseContext _context;
        public ShopService(AppDatabaseContext context)
        {
            _context = context;
        }
        public async Task Delete(int id, string userId)
        {
            Shop? shop = await _context.Shops.FindAsync(id);
            if (shop == null) throw new KeyNotFoundException(Constants.SHOP_NOT_FOUND);

            if (shop.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();
        }

        public async Task<Shop> Get(int id)
        {
            Shop? shop = await _context.Shops.FindAsync(id);

            if (shop is null) throw new KeyNotFoundException(Constants.SHOP_NOT_FOUND);

            return shop;
        }

        public async Task<IEnumerable<Shop>> GetAll()
        {
           return await _context.Shops.AsNoTracking().ToListAsync();
        }

        public async Task<Shop> Post(string userId, Shop shop)
        {
            if (userId != shop.OwnerId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            _context.Shops.Add(shop);
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
            return shop;
        }

        public async Task<Shop> Put(int id, string userId, Shop shop)
        {
            if (shop.ShopId != id) throw new BadHttpRequestException(Constants.INVALID_ID);

            Shop? shop1 = await _context.Shops.AsNoTracking().FirstOrDefaultAsync(s => s.ShopId == id);

            if (shop1 is null) throw new KeyNotFoundException(Constants.SHOP_NOT_FOUND);

            if (shop1.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            shop.UpdatedAt = DateTime.UtcNow;
            _context.Entry(shop).State = EntityState.Modified;
            _context.Entry(shop).Property(s => s.CreatedAt).IsModified = false;


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
            return shop;
        }

        public async Task<ResponseResource<Shop>> GetAllShopsPaginated(string pageNumber, string pageSize)
        {
            IQueryable<Shop> source = _context.Shops.AsNoTracking().Include(s => s.Reductions);
            
            return await PaginationService<Shop>.Paginate(pageNumber, pageSize, source);
        }

        public async Task<ResponseResource<Shop>> GetShopByKeyword(string keyword, string pageNumber, string pageSize)
        {
            keyword = keyword.ToLower().Trim();

            IQueryable<Shop> source = _context.Shops.AsNoTracking().
                Where(s => 
                        s.ShopName.ToLower().Trim().Contains(keyword.ToLower()) ||
                        s.ShopDescription.ToLower().Trim().Contains(keyword.ToLower())
                ).
                Include(s => s.Products).
                Include(s => s.Reductions);

            return await PaginationService<Shop>.Paginate(pageNumber, pageSize, source);
        }

        public async Task<Shop> GetShopByName(string shopName)
        {
            shopName = shopName.ToLower().Trim();
            Shop? shop = await _context.Shops.AsNoTracking().FirstOrDefaultAsync(s => s.ShopName.ToLower().Trim() == shopName);
            if ( shop is null ) throw new KeyNotFoundException(Constants.SHOP_NOT_FOUND);

            return shop;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Shop>> GetMostUsed()
        {
            return await _context.Shops.AsNoTracking().
               OrderByDescending(s => s.Products!.Count()).Take(5).ToListAsync();
        }

        public async Task<ResponseResource<Shop>> GetShopsByUser(string userId, string pageNumber, string pageSize)
        {
            IQueryable<Shop> source = _context.Shops.AsNoTracking().Where(s => s.OwnerId == userId);

            return await PaginationService<Shop>.Paginate(pageNumber, pageSize, source);
        }

        /// <summary>
        /// Catch the exception for unique field
        /// </summary>
        /// <param name="exception"></param>
        /// <exception cref="BadHttpRequestException"></exception>
        private static void HandleException(PostgresException exception)
        {
            if (exception.ConstraintName!.Equals("IX_Shops_ShopName"))
                throw new BadHttpRequestException(Constants.SHOP_EXIST);
        }
    }
}
