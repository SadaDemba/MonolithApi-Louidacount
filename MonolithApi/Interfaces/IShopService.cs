using MonolithApi.Models;
using MonolithApi.Resources.Pagination;

namespace MonolithApi.Interfaces
{
    public interface IShopService: IGeneric<Shop>
    {
        /// <summary>
        /// Get the five Shops with the biggest number of products
        /// </summary>
        /// <returns>A list of shops</returns>
        Task<IEnumerable<Shop>> GetMostUsed();

        public Task<ResponseResource<Shop>> GetAllShopsPaginated(string pageNumber, string pageSize);

        public Task<ResponseResource<Shop>> GetShopsByUser(string userId, string pageNumber, string pageSize);

        public Task<Shop> GetShopByName(string shopName);

        public Task<ResponseResource<Shop>> GetShopByKeyword(string keyword, string pageNumber, string pageSize);

    }
}
