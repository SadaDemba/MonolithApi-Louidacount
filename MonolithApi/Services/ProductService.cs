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
    public class ProductService : IProductService
    {
        private readonly AppDatabaseContext _context;

        public ProductService(AppDatabaseContext context) 
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task Delete(int id, string userId)
        {
            Product? product = await _context.Products.Include(p=>p.Shop).FirstOrDefaultAsync(p=> p.ProductId == id);

            if (product == null) throw new KeyNotFoundException(Constants.PRODUCT_NOT_FOUND);

            if(product.Shop!.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Product> Get(int id)
        {
            Product? product = await _context.Products.
                Include(p => p.Shop).
                Include(p => p.ProductType).
                Include(p => p.ProductReductions!).ThenInclude(pr => pr.Reduction).FirstOrDefaultAsync(p=>p.ProductId == id);
            if (product == null) throw new KeyNotFoundException(Constants.PRODUCT_NOT_FOUND);

            return product;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.AsNoTracking().
                Include(p => p.Shop).
                Include(p => p.ProductType).
                Include(p => p.ProductReductions!).ThenInclude(pr => pr.Reduction).
                OrderBy(p => p.CreatedAt).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ResponseResource<Product>> GetAllPaginated(string pageNumber, string pageSize)
        {
            IQueryable<Product> source = _context.Products.AsNoTracking().
                Include(p => p.ProductType).
                Include(p => p.Shop).
                Include(p => p.ProductReductions!).ThenInclude(pr => pr.Reduction).
                OrderBy(p => p.CreatedAt);

            return await PaginationService<Product>.Paginate(pageNumber, pageSize, source);
        }

        /// <inheritdoc/>
        public async Task<ResponseResource<Product>> GetByShop(int shopId, string pageNumber, string pageSize)
        {
            if (!_context.Shops.Any(s => s.ShopId == shopId))
                throw new KeyNotFoundException(Constants.SHOP_NOT_FOUND);

            IQueryable<Product> source = _context.Products.AsNoTracking().
                Where(p=>p.ShopId == shopId).
                Include(p => p.Shop).
                Include(p => p.ProductType).
                Include(p => p.ProductReductions!).ThenInclude(pr => pr.Reduction).
                OrderBy(p => p.CreatedAt);

            return await PaginationService<Product>.Paginate(pageNumber, pageSize, source);
        }

        /// <inheritdoc/>
        public async Task<ResponseResource<Product>> GetByProductType(int productTypeId, string pageNumber, string pageSize)
        {
            if (!_context.ProductTypes.Any(p => p.Id == productTypeId))
                throw new KeyNotFoundException(Constants.PRODUCT_TYPE_NOT_FOUND);

            IQueryable<Product> source = _context.Products.AsNoTracking().
                Where(p => p.ProductTypeId == productTypeId).
                Include(p => p.Shop).
                Include(p => p.ProductType).
                Include(p => p.ProductReductions!).ThenInclude(pr => pr.Reduction).
                OrderBy(p => p.CreatedAt);
            return await PaginationService<Product>.Paginate(pageNumber, pageSize, source);
        }

        /// <inheritdoc/>
        public async Task<ResponseResource<Product>> GetByPrice(double minValue, double maxValue, string pageNumber, string pageSize)
        {
            if(minValue < 0 || maxValue <= 0 || minValue > maxValue) 
                throw new KeyNotFoundException(Constants.INVALID_VALUES);
            
            IQueryable<Product> source = _context.Products.AsNoTracking().
                Where(p => (p.Price >= minValue && p.Price <= maxValue)).
                Include(p => p.Shop).Include(p => p.ProductType).
                Include(p => p.ProductReductions!).ThenInclude(pr => pr.Reduction).
                OrderBy(p => p.CreatedAt);

            return await PaginationService<Product>.Paginate(pageNumber, pageSize, source);
        }

        /// <inheritdoc/>
        public async Task<Product> Post(string userId, Product product)
        {
            if (!_context.ProductTypes.Any(pt => pt.Id == product.ProductTypeId)) 
                throw new KeyNotFoundException(Constants.PRODUCT_TYPE_NOT_FOUND);
            Shop? shop = _context.Shops.Find(product.ShopId);
            if (shop is null) throw new KeyNotFoundException(Constants.SHOP_NOT_FOUND);

            if (shop.OwnerId != userId) throw new KeyNotFoundException(Constants.ACTION_FORBIDDEN);

            _context.Products.Add(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException is PostgresException postgres)
                {
                    HandleAddressException(postgres);
                }
                throw;
            }
            return product;
        }

        /// <inheritdoc/>
        public async Task<Product> Put(int id, string userId, Product product)
        {
            if (product.ProductId != id) throw new BadHttpRequestException(Constants.INVALID_ID);

            if (!_context.ProductTypes.Any(pt => pt.Id == product.ProductTypeId))
                throw new KeyNotFoundException(Constants.PRODUCT_TYPE_NOT_FOUND);

            if (!_context.Shops.Any(s => s.ShopId == product.ShopId))
                throw new KeyNotFoundException(Constants.SHOP_NOT_FOUND);

            Product? prod = await _context.Products.
                AsNoTracking().FirstOrDefaultAsync(a => a.ProductId == id);

            if (prod is null) throw new KeyNotFoundException(Constants.PRODUCT_NOT_FOUND);

            product.UpdatedAt = DateTime.UtcNow;

            _context.Entry(product).State = EntityState.Modified;
            _context.Entry(product).Property(p => p.CreatedAt).IsModified = false;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException is PostgresException postgres)
                {
                    HandleAddressException(postgres);
                }
                throw;
            }
            return product;
        }

        /// <summary>
        /// Catch the exception for unique field
        /// </summary>
        /// <param name="exception"></param>
        /// <exception cref="BadHttpRequestException"></exception>
        private static void HandleAddressException(PostgresException exception)
        {
            if (exception.ConstraintName!.Equals("IX_ProductTypeId_ShopId_Name")) 
                throw new BadHttpRequestException(Constants.PRODUCT_EXIST);
        }

    }
}
