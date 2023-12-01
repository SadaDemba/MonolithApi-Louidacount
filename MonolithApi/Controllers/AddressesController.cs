using Microsoft.AspNetCore.Mvc;
using MonolithApi.Interfaces;
using MonolithApi.Models;

namespace MonolithApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _ias;

        public AddressesController(IAddressService addressService)
        {
            _ias = addressService;
        }

        // GET: api/Addresses/Paginated?pageNumber=1&pageSize=10
        [HttpGet("Paginated")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAllAddressesPaginated([FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await _ias.GetAllPaginated(pageNumber, pageSize));
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAllAddresses()
        {
            return Ok(await _ias.GetAll());
        }

        // GET: api/Addresses/country?city=Sénégal&pageNumber=1&pageSize=10
        [HttpGet("Country")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddressesByCountry(string country, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            country = country.ToLower().Trim();
            return Ok(await _ias.GetByCountry(country, pageNumber, pageSize));
        }

        // GET: api/Addresses/city/city?city=Dakar&pageNumber=1&pageSize=10
        [HttpGet("City")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddressesByCity( string city, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            city = city.ToLower().Trim();
            return Ok(await _ias.GetByCity(city, pageNumber, pageSize));
        }

        // GET: api/Addresses/keyword?city=Sénégal&pageNumber=1&pageSize=10
        [HttpGet("Keyword")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddressesByKeyword(string value, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await _ias.GetByKeyword(value, pageNumber, pageSize));
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress([FromRoute] int id)
        {
          return await _ias.Get(id);
        }

        // PUT: api/Addresses/5
        //this route is only for administrators
        [HttpPut("Admin/{id}")]
        public async Task<IActionResult> PutAddress([FromRoute] int id, Address address)
        {
           await _ias.Put(id, address);
            return NoContent();
        }

        // POST: api/Addresses
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            Address addr = await _ias.Post(address);
            return Ok(addr);
        }

        // DELETE: api/Addresses/5
        //this route is only for administrators
        [HttpDelete("Admin/{id}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] int id)
        {
           await _ias.Delete(id);

           return NoContent();
        }

    }
}
