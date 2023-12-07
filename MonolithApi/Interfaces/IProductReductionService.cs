using MonolithApi.Models;
using MonolithApi.Resources.Pagination;

namespace MonolithApi.Interfaces
{
    public interface IProductReductionService : IGeneric<ProductReduction>
    {
        Task<IEnumerable<ProductReduction>> GetAllByShop(int shopId);

        /// <summary>
        /// Get the five ProductTypes which are mostly used
        /// </summary>
        /// <returns>A list of product type</returns>
        Task<IEnumerable<ProductReduction>> GetMostUsed();

        Task<ResponseResource<ProductReduction>> GetAllPaginatedByShop(int shopId, string pageNumber, string pageSize);

        Task<ResponseResource<ProductReduction>> GetAllPaginated(string pageNumber, string pageSize);

        Task<ResponseResource<ProductReduction>> GetByProductAndShop(int shopId, int productId, string pageNumber, string pageSize);

        Task<ResponseResource<ProductReduction>> GetByProduct(int productId, string pageNumber, string pageSize);

        Task<ResponseResource<ProductReduction>> GetSwitchIsActivatedByShop(int shopId, bool isActivted, string pageNumber, string pageSize);

        Task<ResponseResource<ProductReduction>> GetSwitchIsActivated(bool isActivted, string pageNumber, string pageSize);

        Task<ResponseResource<ProductReduction>> GetByReduction(int shopId, int reductionId, string pageNumber, string pageSize);

        Task<ProductReduction> SetIsActivated(int id, string userId, bool activate);
    }
}
