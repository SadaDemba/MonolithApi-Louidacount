using Microsoft.AspNetCore.Mvc;
using MonolithApi.Interfaces;
using MonolithApi.Models;

namespace MonolithApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private readonly IProductTypeService @ips;

        public ProductTypesController(IProductTypeService productTypeService)
        {
            ips = productTypeService;
        }

        // GET: api/ProductTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetProductTypes()
        {
            return Ok(await ips.GetAll());
        }

        // GET: api/ProductTypes
        [HttpGet("Paginated")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetProductTypesPaginated([FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await ips.GetAllPaginated(pageNumber, pageSize));
        }

        // GET: api/ProductTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductType>> GetProductType(int id)
        {
          return  Ok(await ips.Get(id));
        }

        // PUT: api/ProductTypes/5
        [HttpPut("Admin/{id}")]
        public async Task<IActionResult> PutProductType(int id, ProductType productType)
        {
            await ips.Put(id, productType);

            return NoContent();
        }

        // POST: api/ProductTypes
        [HttpPost("Seller")]
        public async Task<ActionResult<ProductType>> PostProductType(ProductType productType)
        {
          ProductType prodType = await ips.Post(productType);

            return CreatedAtAction("GetProductType", new { id = prodType.Id }, prodType);
        }

        // DELETE: api/ProductTypes/5
        //this route is only for administrators
        [HttpDelete("Admin/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            await ips.Delete(id);

            return NoContent();
        }

        // GET: api/ProductTypes/name?value=flower
        [HttpGet("name")]
        public async Task<ActionResult<ProductType>> GetProductTypeByName([FromQuery] string value)
        {
            value = value.ToLower();
            return Ok(await ips.GetByName(value));
        }


    }
}
