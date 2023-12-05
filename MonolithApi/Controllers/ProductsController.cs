using Microsoft.AspNetCore.Mvc;
using MonolithApi.Interfaces;
using MonolithApi.Models;

namespace MonolithApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService @ips;

        public ProductsController(IProductService productService)
        {
            ips = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await ips.GetAll());
        }

        // GET: api/Products/Paginated?pageNumber=1&pageSize=10
        [HttpGet("Paginated")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await ips.GetAllPaginated(pageNumber, pageSize));
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          return await ips.Get(id);
        }

        //GET: api/Products/Shop/1?pageNumber=1&pageSize=10
        [HttpGet("Shop/{shopId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByShop(int shopId, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await ips.GetByShop(shopId, pageNumber, pageSize));
        }

        //GET: api/Products/Keyword?keyword=Rose&pageNumber=1&pageSize=10
        [HttpGet("Keyword/")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByKeyword([FromQuery] string keyword, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await ips.GetByKeyword(keyword, pageNumber, pageSize));
        }

        //GET: api/Products/ProdType/1?pageNumber=1&pageSize=10
        [HttpGet("ProdType/{productTypeId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByType(int productTypeId, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await ips.GetByProductType(productTypeId, pageNumber, pageSize));
        }

        //GET: api/Products/Price?minValue=100&max&Value=1000&pageNumber=1&pageSize=10
        [HttpGet("Price")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPrice([FromQuery] double minValue, [FromQuery] double maxValue, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await ips.GetByPrice(minValue,maxValue, pageNumber, pageSize));
        }

        // PUT: api/Products/5
        [HttpPut("Seller/{userId}/{id}")]
        public async Task<IActionResult> PutProduct(int id, string userId, Product product)
        {
            await ips.Put(id, userId, product);
            return NoContent();
        }

        // POST: api/Products
        [HttpPost("Seller/{userId}")]
        public async Task<ActionResult<Product>> PostProduct(string userId, Product product)
        {
          Product prod = await ips.Post(userId, product);

            return CreatedAtAction("GetProduct", new { id = prod.ProductId }, prod);
        }

        // DELETE: api/Products/5
        [HttpDelete("Seller/{userId}/{id}")]
        public async Task<IActionResult> DeleteProduct(int id, string userId)
        {
            await ips.Delete(id, userId);

            return NoContent();
        }






    }
}
