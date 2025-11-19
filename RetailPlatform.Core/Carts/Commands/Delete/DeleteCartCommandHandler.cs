using MassTransit;
using Microsoft.Extensions.Logging;
using RetailPlatform.Contracts;
using RetailPlatform.Core.Contracts;

namespace RetailPlatform.Core.Carts.Commands.Delete
{
    public class DeleteCartCommandHandler(ICartRepository cartRepository, IPublishEndpoint publish, ILogger<DeleteCartCommandHandler> logger) : IDeleteCartCommandHandler
    {

        public async Task HandleAsync(DeleteCartCommand command, CancellationToken cancellationToken = default)
        {
            var cart = await cartRepository.GetByIdAsync(command.CartId);
            if (cart is null)
            {
                logger.LogWarning("Cart with id {CartId} not found; nothing to delete.", command.CartId);
                return;
            }

            try
            {
                await cartRepository.DeleteAsync(cart.Id, cancellationToken);
                await cartRepository.SaveChangesAsync(cancellationToken);

                await publish.Publish(new CartDeletedEvent
                {
                    CartId = cart.Id
                }, cancellationToken);

            }
            catch (Exception ex)
            {
                logger.LogError("Cart Delete failed. Error: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}