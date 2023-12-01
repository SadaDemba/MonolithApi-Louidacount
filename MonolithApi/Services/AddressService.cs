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
    public class AddressService : IAddressService
    {
        private readonly AppDatabaseContext _context;

        /// <summary>
        /// This is the constructor of this class
        /// </summary>
        /// <param name="context"></param>
        public AddressService(AppDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Address> Post(Address address)
        {

            _context.Addresses.Add(address);
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
            return address;
        }

        /// <inheritdoc/>
        public async Task Delete(int id)
        {
            Address? address = await _context.Addresses.FindAsync(id);
            if (address is null) throw new KeyNotFoundException(Constants.ADDRESS_NOT_FOUND);
           
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Address> Put(int id, Address address) 
        { 
            if(address.AddressId != id) throw new BadHttpRequestException(Constants.INVALID_ID);

            Address? address1 = await _context.Addresses.
                AsNoTracking().
                FirstOrDefaultAsync(a=>a.AddressId == id);

            if (address1 is null) throw new KeyNotFoundException(Constants.ADDRESS_NOT_FOUND);
            address.UpdatedAt = DateTime.UtcNow;
            _context.Entry(address).State = EntityState.Modified;
            _context.Entry(address).Property(a => a.CreatedAt).IsModified = false;


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
            return address;
        }

        /// <inheritdoc/>
        public async Task<Address> Get(int id)
        {
            Address? address = await _context.Addresses.FindAsync(id);

            if (address is null) throw new KeyNotFoundException(Constants.ADDRESS_NOT_FOUND);

            return address;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Address>> GetAll()
        {
            return await _context.Addresses.AsNoTracking().OrderBy(a => a.Country).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ResponseResource<Address>> GetAllPaginated(string pageNumber, string pageSize)
        {
            IQueryable<Address> source =  _context.Addresses.AsNoTracking().OrderBy(a=>a.Country);

            return await PaginationService<Address>.Paginate(pageNumber, pageSize, source);
        }

        /// <inheritdoc/>
        public async Task<ResponseResource<Address>> GetByCountry(string country, string pageNumber, string pageSize)
        {
            IQueryable<Address> source = _context.Addresses.AsNoTracking().
                Where(a=>a.Country.ToLower().Trim() == country).OrderBy(a => a.Country);

            return await PaginationService<Address>.Paginate(pageNumber, pageSize, source);
        }

        /// <inheritdoc/>
        public async Task<ResponseResource<Address>> GetByCity(String city, string pageNumber, string pageSize)
        {
            IQueryable<Address> source = _context.Addresses.AsNoTracking().
                Where(a => a.City.ToLower().Trim() == city).OrderBy(a => a.Country);

            return await PaginationService<Address>.Paginate(pageNumber, pageSize, source); 
        }
        /// <inheritdoc/>
        public async Task<ResponseResource<Address>> GetByKeyword(string keyword, string pageNumber, string pageSize)
        {
            keyword = keyword.ToLower().Trim();
            IQueryable<Address> source = _context.Addresses.AsNoTracking().
                Where(a =>
                        a.Street.ToLower().Trim().Contains(keyword) ||
                        a.City.ToLower().Trim().Contains(keyword) ||
                        a.Country.ToLower().Trim().Contains(keyword) ||
                        a.StreetNumber.ToString().Contains(keyword) ||
                        a.PostalCode.ToString().Contains(keyword)

                ).
                OrderBy(a => a.Country);

            return await PaginationService<Address>.Paginate(pageNumber, pageSize, source);
        }

        /// <summary>
        /// Catch the exception for unique fields key
        /// </summary>
        /// <param name="exception"></param>
        /// <exception cref="BadHttpRequestException"></exception>
        private static void HandleAddressException(PostgresException exception)
        {
            if (exception.ConstraintName!.Equals("IX_Addresses_Street_StreetNumber_Country_City_PostalCode")) throw new BadHttpRequestException(Constants.ADDRESS_EXIST);
        }
    }
}
