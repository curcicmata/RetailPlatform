using MassTransit;
using RetailPlatform.Contracts;
using RetailPlatform.Core.Contracts;
using RetailPlatform.Core.DTOs;
using RetailPlatform.Domain.Models;

namespace RetailPlatform.Core.Carts.Commands
{
    public class UpsertCartCommandHandler(ICartRepository cartRepository, IPublishEndpoint publish) : IUpsertCartCommandHandler
    {
        public async Task<CartDto> HandleAsync(UpsertCartCommand command, CancellationToken cancellationToken = default)
        {
            var existingCart = await cartRepository.GetByUserIdAsync(command.UserId);
            if (existingCart is null)
            {
                existingCart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = command.UserId,
                    UpdatedAt = DateTime.UtcNow,
                    Items = []
                };
            }

            existingCart.Items.Clear();

            foreach (var item in command.Items)
            {
                existingCart.Items.Add(new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = existingCart.Id,
                    Sku = item.Sku,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            await cartRepository.UpsertAsync(existingCart, cancellationToken);

            await publish.Publish(new CartUpdatedEvent
            {
                CartId = existingCart.Id,
                UserId = existingCart.UserId,
                Items = existingCart.Items.Select(item => new CartItemEventDto
                {
                    Sku = item.Sku,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            }, cancellationToken);

            var cartDto = CartDto.CreateDto(existingCart);

            return cartDto;
        }
    }
}