using Microsoft.AspNetCore.Mvc;
using RetailPlatform.Core.Carts.Queries;

namespace RetailPlatform.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController(IGetCartQueryHandler handler) : ControllerBase
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var query = new GetCartQuery { UserId = userId };
            var result = await handler.HandleAsync(query);

            return Ok(result);
        }
    }
}
