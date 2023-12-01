using MonolithApi.Models;
using MonolithApi.Resources.Pagination;

namespace MonolithApi.Interfaces
{
    public interface IReductionService : IGeneric<Reduction>
    {
        /// <summary>
        /// Retrieve all Reductions paginated
        /// </summary>
        /// <param name="pageNumber">The number of the page we want to retrieve</param>
        /// <param name="pageSize">The size of our pages</param>
        /// <returns>List of reduction paginated</returns>
        Task<ResponseResource<Reduction>> GetAllPaginated(string pageNumber, string pageSize);

        /// <summary>
        /// Retrieve all the reductions of a given shop
        /// </summary>
        /// <param name="shopId">Id of the shop which we want to retrieve reductions</param>
        /// <param name="userId">The id of the user which do the request</param>
        /// <param name="pageNumber">The number of the page we want to retrieve</param>
        /// <param name="pageSize">The size of our pages</param>
        /// <returns>List of reduction paginated</returns>
        Task<ResponseResource<Reduction>> GetAllPaginatedByShop(int shopId, string userId, string pageNumber, string pageSize);

        /// <summary>
        /// Retrieve all reductions according to the fact that they are active or not
        /// </summary>
        /// <param name="isActive">The variable which specify if we want active or inactive reductions</param>
        /// <param name="pageNumber">The number of the page we want to retrieve</param>
        /// <param name="pageSize">The size of our pages</param>
        /// <returns>List of reduction paginated</returns>
       // Task<ResponseResource<Reduction>> GetInCurrentPeriod(bool isActive,  string pageNumber, string pageSize);

        /// <summary>
        /// Retrieve all reductions for a shop according to the fact that they are active or not
        /// </summary>
        /// <param name="shopId">Id of the shop which we want to retrieve reductions</param>
        /// <param name="isActive">The variable which specify if we want active or inactive reductions</param>
        /// <param name="pageNumber">The number of the page we want to retrieve</param>
        /// <param name="pageSize">The size of our pages</param>
        /// <returns>List of reduction paginated</returns>
       // Task<ResponseResource<Reduction>> GetInCurrentPeriodByShop(int shopId, bool isActive, string pageNumber, string pageSize);
    }
}
