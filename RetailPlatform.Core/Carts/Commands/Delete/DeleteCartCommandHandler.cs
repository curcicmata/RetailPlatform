using MassTransit;
using RetailPlatform.Contracts;
using RetailPlatform.Core.Contracts;

namespace RetailPlatform.Core.Carts.Commands.Delete
{
    public class DeleteCartCommandHandler(ICartRepository cartRepository, IPublishEndpoint publish) : IDeleteCartCommandHandler
    {

        public async Task HandleAsync(DeleteCartCommand command, CancellationToken cancellationToken = default)
        {
            var cart = await cartRepository.GetByIdAsync(command.CartId);
            if (cart is null)
                return;


            await cartRepository.DeleteAsync(cart.Id, cancellationToken);
            await cartRepository.SaveChangesAsync(cancellationToken);

            await publish.Publish(new CartDeletedEvent
            {
                CartId = cart.Id
            }, cancellationToken);
        }
    }
}