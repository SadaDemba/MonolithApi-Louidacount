using Microsoft.AspNetCore.WebUtilities;
using MonolithApi.Resources.Pagination;

namespace MonolithApi.Services.Pagination
{
    public class PageUrlService
    {
        public PageUrlService()
        {

        }
        public static string GetPageUrl(PaginationFilter filter)
        {
            var modifiedUri = QueryHelpers.AddQueryString("", "pageNumber", filter.PageNumber.ToString());
            return QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
        }
    }
}
