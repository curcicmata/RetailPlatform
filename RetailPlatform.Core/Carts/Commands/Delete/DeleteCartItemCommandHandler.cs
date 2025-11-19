using MassTransit;
using RetailPlatform.Contracts;
using RetailPlatform.Core.Contracts;

namespace RetailPlatform.Core.Carts.Commands.Delete
{
    public class DeleteCartItemCommandHandler(ICartRepository cartRepository, IPublishEndpoint publish) : IDeleteCartItemCommandHandler
    {
        public async Task HandleAsync(DeleteCartItemCommand command, CancellationToken cancellationToken = default)
        {
            var cart = await cartRepository.GetByIdAsync(command.CartId);
            if (cart is null)
                return;

            var item = cart.Items.FirstOrDefault(i => i.Id == command.ItemId);
            if (item is null)
                return;

            cart.Items.Remove(item);

            await cartRepository.UpdateAsync(cart);
            await cartRepository.SaveChangesAsync(cancellationToken);

            await publish.Publish(new CartItemDeletedEvent
            {
                CartId = command.CartId,
                ItemId = command.ItemId
            }, cancellationToken);
        }
    }
}
