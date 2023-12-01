using AutoBogus;
using MonolithApi.Context;
using MonolithApi.Models;

namespace MonolithApi.Data
{
    public class DBInitializer
    {

        public static void Initialize( AppDatabaseContext context)
        {
            if(!context.Addresses.Any())
            {
                var addressFaker = new AutoFaker<Address>();

                var addresses = addressFaker.Generate(15);

                context.Addresses.AddRange(addresses);
                context.SaveChanges();
            }
        }
    }
}
