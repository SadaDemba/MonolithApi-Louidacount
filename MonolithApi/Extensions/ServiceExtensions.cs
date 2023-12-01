using MonolithApi.Interfaces;
using MonolithApi.Services;

namespace MonolithApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterAppServices(this IServiceCollection collection)
        {
            collection.AddScoped<IAddressService, AddressService>();
            collection.AddScoped<IProductTypeService, ProductTypeService>();
            collection.AddScoped<IProductService, ProductService>();
            collection.AddScoped<IShopService, ShopService>();
            collection.AddScoped<IProductReductionService, ProductReductionService>();
            collection.AddScoped<IReductionService, ReductionService>();
        }
    }
}
