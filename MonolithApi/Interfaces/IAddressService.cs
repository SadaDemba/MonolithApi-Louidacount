using MonolithApi.Models;
using MonolithApi.Resources.Pagination;

namespace MonolithApi.Interfaces
{
    public interface IAddressService
    {
        /// <summary>
        /// Add a new Address
        /// </summary>
        /// <param name="t">The object we want to add</param>
        /// <returns>The object</returns>
        Task<Address> Post(Address address);

        /// <summary>
        /// Update an object
        /// </summary>
        /// <param name="id">Id of the object we want to update</param>
        /// <returns>A single address</returns>
        Task<Address> Put(int id, Address address);

        /// <summary>
        /// Delete an object
        /// </summary>
        /// <param name="id">Id of the object we want to delete</param>
        Task Delete(int id);

        /// <summary>
        /// Get an object
        /// </summary>
        /// <param name="id">Id of the object we want to retrieve</param>
        /// <returns>A single object</returns>
        Task<Address> Get(int id);

        /// <summary>
        /// Get All objects
        /// </summary>
        /// <returns>A list of objects</returns>
        Task<IEnumerable<Address>> GetAll();

        /// <summary>
        /// Get All objects
        /// </summary>
        /// <param name="pageNumber">Number of the page we want to retrieve</param>
        /// <param name="pageSize">size of our pages</param>
        /// <returns>A list of objects</returns>
        Task<ResponseResource<Address>> GetAllPaginated(string pageNumber, string pageSize);

        /// <summary>
        /// Get addresses by country
        /// </summary>
        /// <param name="country">The country of addresses we want to retrieve</param>
        /// <param name="pageNumber">Number of the page we want to retrieve</param>
        /// <param name="pageSize">size of our pages</param>
        /// <returns>A list of addresses</returns>
        Task<ResponseResource<Address>> GetByCountry(String country, string pageNumber, string pageSize);


        /// <summary>
        /// Get addresses by city
        /// </summary>
        /// <param name="city">The city of addresses we want to retrieve</param>
        /// <param name="pageNumber">Number of the page we want to retrieve</param>
        /// <param name="pageSize">size of our pages</param>
        /// <returns>A list of addresses</returns>
        Task<ResponseResource<Address>> GetByCity(String city, string pageNumber, string pageSize);


        /// <summary>
        /// Get addresses by key word
        /// </summary>
        /// <param name="keyWord">The keyWord which is in addresses we want to retrieve</param>
        /// <param name="pageNumber">Number of the page we want to retrieve</param>
        /// <param name="pageSize">size of our pages</param>
        /// <returns>A list of addresses</returns>
        Task<ResponseResource<Address>> GetByKeyword(String keyWord, string pageNumber, string pageSize);


    }
}
