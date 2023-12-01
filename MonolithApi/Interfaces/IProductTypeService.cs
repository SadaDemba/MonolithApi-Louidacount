using MonolithApi.Models;
using MonolithApi.Resources.Pagination;

namespace MonolithApi.Interfaces
{
    public interface IProductTypeService
    {
        /// <summary>
        /// Get product type by name
        /// </summary>
        /// <param name="city">The name of the product type we want to retrieve</param>
        /// <returns>A product type</returns>
        Task<ProductType> GetByName(string name);

        /// <summary>
        /// Get all productType by page
        /// </summary>
        /// <param name="pageNumber">Number of the page we want to retrieve</param>
        /// <param name="pageSize">Size of our pages</param>
        /// <returns></returns>
        Task<ResponseResource<ProductType>> GetAllPaginated(string pageNumber, string pageSize);

        /// <summary>
        /// Add a new type of product. Action allowed to admin and sellers
        /// </summary>
        /// <param name="productType">The object we want to add</param>
        /// <returns>The object</returns>
        Task<ProductType> Post(ProductType productType);

        /// <summary>
        /// Update a type of product. Action allowed only to admin
        /// </summary>
        /// <param name="id">Id of the object we want to update</param>
        /// <returns>A single address</returns>
        Task<ProductType> Put(int id, ProductType t);

        /// <summary>
        /// Delete a type of product! Allowed to admin but not recommended
        /// </summary>
        /// <param name="id">Id of the object we want to delete</param>
        Task Delete(int id);

        /// <summary>
        /// Get a type of product
        /// </summary>
        /// <param name="id">Id of the object we want to retrieve</param>
        /// <returns>A single object</returns>
        Task<ProductType> Get(int id);

        /// <summary>
        /// Get All types of product
        /// </summary>
        /// <returns>A list of objects</returns>
        Task<IEnumerable<ProductType>> GetAll();
    }
}
