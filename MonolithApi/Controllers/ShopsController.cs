using Microsoft.AspNetCore.Mvc;
using MonolithApi.Interfaces;
using MonolithApi.Models;
using MonolithApi.Resources.Pagination;

namespace MonolithApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IShopService _iss;

        public ShopsController(IShopService shopService)
        {
            _iss = shopService;
        }

        // GET: api/Shops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shop>>> GetShops()
        {
           return Ok(await _iss.GetAll());
        }

        // GET: api/Shops/Paginated
        [HttpGet("Paginated")]
        public async Task<ActionResult<ResponseResource<Shop>>> GetShopsPaginated([FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await _iss.GetAllShopsPaginated(pageNumber, pageSize));
        }

        // GET: api/Shops/Paginated/Keyword
        [HttpGet("Paginated/Keyword")]
        public async Task<ActionResult<ResponseResource<Shop>>> GetShopsByKeyword([FromQuery] string keyword, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await _iss.GetShopByKeyword(keyword, pageNumber, pageSize));
        }

        // GET: api/Shops/ShopName
        [HttpGet("ShopName/")]
        public async Task<ActionResult<Shop>> GetShopByName(string name)
        {
            return Ok(await _iss.GetShopByName(name));
        }

        // GET: api/Shops/Owner
        [HttpGet("Seller/Owner")]
        public async Task<ActionResult<ResponseResource<Shop>>> GetShopByOwner(string userId, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await _iss.GetShopsByUser(userId, pageNumber, pageSize));
        }

        // GET: api/ProductTypes
        [HttpGet("MostUsed")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetMostUsedProductTypes()
        {
            return Ok(await _iss.GetMostUsed());
        }

        // GET: api/Shops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> GetShop(int id)
        {
          return Ok(await _iss.Get(id));
        }

        // PUT: api/Shops/5
        [HttpPut("Seller/{userId}/{id}")]
        public async Task<IActionResult> PutShop(int id, string userId, Shop shop)
        {
            await _iss.Put(id, userId, shop);
            return NoContent();

        }

        // POST: api/Shops
        [HttpPost("Seller/{userId}")]
        public async Task<ActionResult<Shop>> PostShop(string userId, Shop shop)
        {
            Shop s = await _iss.Post(userId, shop);

            return CreatedAtAction("GetShop", new { id = s.ShopId }, s);
        }

        // DELETE: api/Shops/5
        [HttpDelete("Seller/{userId}/{id}")]
        public async Task<IActionResult> DeleteShop(int id, string userId)
        {
            await _iss.Delete(id, userId);
            return NoContent();
        }


    }
}
