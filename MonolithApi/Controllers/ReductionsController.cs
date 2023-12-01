using Microsoft.AspNetCore.Mvc;
using MonolithApi.Interfaces;
using MonolithApi.Models;
using MonolithApi.Resources.Pagination;

namespace MonolithApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReductionsController : ControllerBase
    {
        private readonly IReductionService @irs;

        public ReductionsController(IReductionService reductionService)
        {
            @irs = reductionService;
        }

        // GET: api/Reductions
        //this route is only for administrators
        [HttpGet("Admin/Paginated")]
        public async Task<ActionResult<ResponseResource<Reduction>>> GetReductionsPaginated([FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await irs.GetAllPaginated(pageNumber, pageSize));
        }

        // GET: api/Reductions/5
        //this route is only for administrators
        [HttpGet("Admin/{id}")]
        public async Task<ActionResult<Reduction>> GetReduction(int id)
        {
            return Ok(await irs.Get(id));
        }

        // GET: api/Reductions
        [HttpGet("Seller/{userId}/Paginated/Shop/{shopId}")]
        public async Task<ActionResult<ResponseResource<Reduction>>> GetReductionsPaginatedByShop(int shopId, string userId, [FromQuery] string pageNumber, [FromQuery] string pageSize)
        {
            return Ok(await irs.GetAllPaginatedByShop(shopId, userId, pageNumber, pageSize));
        }


        // PUT: api/Reductions/5
        [HttpPut("Seller/{userId}/{id}")]
        public async Task<IActionResult> PutReduction(int id, string userId, Reduction reduction)
        {
            await irs.Put(id, userId, reduction);

            return NoContent();
        }

        // POST: api/Reductions
        [HttpPost("Seller/{userId}")]
        public async Task<ActionResult<Reduction>> PostReduction(string userId, Reduction reduction)
        {
          Reduction r = await irs.Post(userId, reduction);

            return CreatedAtAction("GetReduction", new { id = r.Id }, r);
        }

        // DELETE: api/Reductions/5
        [HttpDelete("Seller/{userId}/{id}")]
        public async Task<IActionResult> DeleteReduction(int id, string userId)
        {
            await irs.Delete(id, userId);
            return NoContent();
        }

    }
}
