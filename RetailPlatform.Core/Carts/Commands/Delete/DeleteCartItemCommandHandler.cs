using MassTransit;
using Microsoft.Extensions.Logging;
using RetailPlatform.Contracts;
using RetailPlatform.Core.Contracts;

namespace RetailPlatform.Core.Carts.Commands.Delete
{
    public class DeleteCartItemCommandHandler(ICartRepository cartRepository, IPublishEndpoint publish, ILogger<DeleteCartItemCommandHandler> logger) : IDeleteCartItemCommandHandler
    {
        public async Task HandleAsync(DeleteCartItemCommand command, CancellationToken cancellationToken = default)
        {
            var cart = await cartRepository.GetByIdAsync(command.CartId);
            if (cart is null)
            {
                logger.LogWarning("Cart with id {CartId} not found; nothing to delete.", command.CartId);
                return;
            }

            var item = cart.Items.FirstOrDefault(i => i.Id == command.ItemId);
            if (item is null)
            {
                logger.LogWarning("Item with id {ItemId} not found in cart {CartId}; nothing to delete.", command.ItemId, command.CartId);
                return;
            }
            try
            {
                cart.Items.Remove(item);

                await cartRepository.UpdateAsync(cart);
                await cartRepository.SaveChangesAsync(cancellationToken);

                await publish.Publish(new CartItemDeletedEvent
                {
                    CartId = command.CartId,
                    ItemId = command.ItemId
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("Cart item delete failed. Error: {ErrorMessage}", ex.Message);
                throw;
            }

        }
    }
}
