using Microsoft.AspNetCore.Mvc;
using MonolithApi.Interfaces;
using MonolithApi.Models;
using MonolithApi.Resources.Pagination;

namespace MonolithApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReductionsController : ControllerBase
    {
        private readonly IProductReductionService @iprs;

        public ProductReductionsController(IProductReductionService productReduction)
        {
            iprs = productReduction;
        }

        // GET: api/
        [HttpGet("MostUsed")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetMostUsedProdReduc()
        {
            return Ok(await iprs.GetMostUsed());
        }

        // GET: api/ProductReductions/Shop/1
        [HttpGet("Shop/{shopId}")]
        public async Task<ActionResult<IEnumerable<ProductReduction>>> GetProductReductionsByShop(int shopId)
        {
            return Ok(await iprs.GetAllByShop(shopId));
        }

        // GET: api/ProductReductions/Shop/1/Paginated?pageNumber=1&pageSize=10
        [HttpGet("Paginated/Shop/{shopId}")]
        public async Task<ActionResult<ResponseResource<ProductReduction>>> GetPaginatedProductReductionsByshop(int shopId, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await iprs.GetAllPaginatedByShop(shopId, pageNumber, pageSize));
        }

        [HttpGet("Paginated")]
        public async Task<ActionResult<ResponseResource<ProductReduction>>> GetPaginatedProductReductions([FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await iprs.GetAllPaginated(pageNumber, pageSize));
        }

        // GET: api/ProductReductions/Shop/1/Paginated?pageNumber=1&pageSize=10
        [HttpGet("Shop/{shopId}/Product/{productId}")]
        public async Task<ActionResult<ResponseResource<ProductReduction>>> GetPaginatedProductByProductAndShop(int shopId, int productId, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await iprs.GetByProductAndShop(shopId, productId, pageNumber, pageSize));
        }

        // GET: api/ProductReductions/Paginated?pageNumber=1&pageSize=10
        [HttpGet("Product/{productId}")]
        public async Task<ActionResult<ResponseResource<ProductReduction>>> GetPaginatedProductReductionByProduct(int productId, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await iprs.GetByProduct(productId, pageNumber, pageSize));
        }

        // GET: api/ProductReductions/Shop/1/Paginated?pageNumber=1&pageSize=10
        [HttpGet("IsActivated")]
        public async Task<ActionResult<ResponseResource<ProductReduction>>> GetPaginatedProductReductionSwitchIsActivated([FromQuery] bool isActivated, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await iprs.GetSwitchIsActivated(isActivated, pageNumber, pageSize));
        }

        // GET: api/ProductReductions/Shop/1/Paginated?pageNumber=1&pageSize=10
        [HttpGet("Shop/{shopId}/IsActivated")]
        public async Task<ActionResult<ResponseResource<ProductReduction>>> GetPaginatedProductReductionSwitchIsActivatedByShop(int shopId, [FromQuery] bool isActivated, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await iprs.GetSwitchIsActivatedByShop(shopId, isActivated, pageNumber, pageSize));
        }

        // GET: api/ProductReductions/5
        [HttpGet("Reduction/{id}")]
        public async Task<ActionResult<ProductReduction>> GetProductReductionByReduction(int reductionId, int shopId, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await iprs.GetByReduction(reductionId, shopId, pageNumber, pageSize));
        }

        // GET: api/ProductReductions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReduction>> GetProductReduction(int id)
        {
            return Ok(await iprs.Get(id));
        }

        // PUT: api/ProductReductions/5
        [HttpPut("Seller/{userId}/SetIsActivated/{id}")]
        public async Task<IActionResult> PutProductReduction(int id, string userId,[FromQuery] bool isActivated)
        {
            await iprs.SetIsActivated(id, userId, isActivated);
            return NoContent();
        }

        // PUT: api/ProductReductions/5
        [HttpPut("Seller/{userId}/{id}")]
        public async Task<IActionResult> PutProductReduction(int id, string userId, ProductReduction productReduction)
        {
            await iprs.Put(id, userId, productReduction);
            return NoContent();
        }

        // POST: api/ProductReductions
        [HttpPost("Seller/{userId}")]
        public async Task<ActionResult<ProductReduction>> PostProductReduction(string userId, ProductReduction productReduction)
        {
            ProductReduction prodReduc = await iprs.Post(userId, productReduction);

            return CreatedAtAction("GetProductReduction", new { id = prodReduc.Id }, prodReduc);
        }

        // DELETE: api/ProductReductions/5
        [HttpDelete("Seller/{userId}/{id}")]
        public async Task<IActionResult> DeleteProductReduction(int id, string userId)
        {
            await iprs.Delete(id, userId);
            return NoContent();
        }
    }
}
