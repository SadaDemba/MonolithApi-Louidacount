using MonolithApi.Models;
using MonolithApi.Resources.Pagination;

namespace MonolithApi.Interfaces
{
    public interface IProductService : IGeneric<Product>
    {
        /// <summary>
        /// Get all products
        /// </summary>
        /// <param name="pageNumber">Number of the page we want to retrieve</param>
        /// <param name="pageSize">size of our pages</param>
        /// <returns>List of product</returns>
        Task<ResponseResource<Product>> GetAllPaginated(string pageNumber, string pageSize);

        /// <summary>
        /// Get products by shop
        /// </summary>
        /// <param name="shopId">The id of the shop which we want to retrieve products</param>
        /// <param name="pageNumber">Number of the page we want to retrieve</param>
        /// <param name="pageSize">size of our pages</param>
        /// <returns>List of product</returns>
        Task<ResponseResource<Product>> GetByShop(int shopId, string pageNumber, string pageSize);

        /// <summary>
        /// Get products by product type
        /// </summary>
        /// <param name="shopId">The id of the product type which we want to retrieve products</param>
        /// <param name="pageNumber">Number of the page we want to retrieve</param>
        /// <param name="pageSize">size of our pages</param>
        /// <returns>List of product</returns>
        Task<ResponseResource<Product>> GetByProductType(int productTypeId, string pageNumber, string pageSize);

        /// <summary>
        /// Get products by price interval
        /// </summary>
        /// <param name="minValue">the price minimum of the products we want to retrieve</param>
        /// <param name="maxValue">the price maximum of the products we want to retrieve</param>
        /// <param name="pageNumber">Number of the page we want to retrieve</param>
        /// <param name="pageSize">size of our pages</param>
        /// <returns>List of product</returns>
        Task<ResponseResource<Product>> GetByPrice(double minValue, double maxValue, string pageNumber, string pageSize);
    }
}
