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
    public class ProductTypeService : IProductTypeService
    {

        private readonly AppDatabaseContext _context;

        public ProductTypeService(AppDatabaseContext context) 
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task Delete(int id)
        {
            ProductType? prodType = await _context.ProductTypes.FindAsync(id);
            if (prodType is null) throw new KeyNotFoundException(Constants.PRODUCT_TYPE_NOT_FOUND);

            if (_context.Products.Any(p => p.ProductTypeId == id))
                throw new KeyNotFoundException(Constants.DEPENDENCY_ERROR);
            _context.ProductTypes.Remove(prodType);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<ProductType> Get(int id)
        {
            ProductType? prodType = await _context.ProductTypes.FindAsync(id);

            if (prodType is null) throw new KeyNotFoundException(Constants.PRODUCT_TYPE_NOT_FOUND);

            return prodType;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProductType>> GetAll()
        {
            return await _context.ProductTypes.AsNoTracking().
                OrderBy(pt => pt.Name).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ResponseResource<ProductType>> GetAllPaginated(string pageNumber, string pageSize)
        {
            IQueryable<ProductType> source = _context.ProductTypes.
                AsNoTracking().OrderBy(pt => pt.Name);
            return await PaginationService<ProductType>.Paginate(pageNumber, pageSize, source);
        }

        public async Task<ProductType> GetByName(String name)
        {
            ProductType? productType = await _context.
                ProductTypes.FirstOrDefaultAsync(pt => pt.Name.ToLower() == name);

            if (productType is null) throw new KeyNotFoundException(Constants.PRODUCT_TYPE_NOT_FOUND);

            return productType;
        }

        /// <inheritdoc/>
        public async Task<ProductType> Post(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
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
            return productType;
        }

        /// <inheritdoc/>
        public async Task<ProductType> Put(int id, ProductType productType)
        {
            if (productType.Id != id) throw new BadHttpRequestException(Constants.INVALID_ID);

            ProductType? prodType = await _context.ProductTypes.
                AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

            if (prodType is null) throw new KeyNotFoundException(Constants.PRODUCT_TYPE_NOT_FOUND);
            
            productType.UpdatedAt = DateTime.UtcNow;

            _context.Entry(productType).State = EntityState.Modified;
            _context.Entry(productType).Property(a => a.CreatedAt).IsModified = false;


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
            return productType;
        }

        /// <summary>
        /// Catch the exception for unique field
        /// </summary>
        /// <param name="exception"></param>
        /// <exception cref="BadHttpRequestException"></exception>
        private static void HandleException(PostgresException exception)
        {
            if (exception.ConstraintName!.Equals("IX_ProductTypes_Name")) 
                throw new BadHttpRequestException(Constants.PRODUCT_TYPE_EXIST);
        }
    }
}
