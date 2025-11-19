using Microsoft.AspNetCore.Mvc;
using RetailPlatform.Core.Carts.Commands.Delete;
using RetailPlatform.Core.Carts.Commands.Upsert;
using RetailPlatform.Core.Carts.Queries;

namespace RetailPlatform.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController(
        IGetCartQueryHandler handler, 
        IUpsertCartCommandHandler cartCommandHandler, 
        IDeleteCartCommandHandler deleteCartCommandHandler, 
        IDeleteCartItemCommandHandler deleteCartItemCommandHandler) : ControllerBase
    {
        /// <summary>
        /// Get user's cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var query = new GetCartQuery { UserId = userId };
            var result = await handler.HandleAsync(query);

            return Ok(result);
        }

        /// <summary>
        /// Update or insert a new/existing cart for a user
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] UpsertCartCommand command, CancellationToken ct)
        {
            var result = await cartCommandHandler.HandleAsync(command, ct);

            return Ok(result);
        }

        /// <summary>
        /// Delete selected cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpDelete("{cartId}")]
        public async Task<IActionResult> Delete(Guid cartId, CancellationToken ct)
        {
            var command = new DeleteCartCommand { CartId = cartId };
            await deleteCartCommandHandler.HandleAsync(command, ct);

            return NoContent();
        }

        /// <summary>
        /// Delete an item from the cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="itemId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpDelete("{cartId}/items/{itemId}")]
        public async Task<IActionResult> DeleteItem(Guid cartId, Guid itemId, CancellationToken ct)
        {
            var command = new DeleteCartItemCommand { CartId = cartId, ItemId = itemId };
            await deleteCartItemCommandHandler.HandleAsync(command, ct);

            return NoContent();
        }
    }
}
