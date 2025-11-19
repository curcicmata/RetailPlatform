using MassTransit;
using RetailPlatform.Contracts;
using RetailPlatform.Core.Contracts;
using RetailPlatform.Core.DTOs;
using RetailPlatform.Domain.Models;

namespace RetailPlatform.Core.Carts.Commands.Upsert
{
    public class UpsertCartCommandHandler(ICartRepository cartRepository, IPublishEndpoint publish) : IUpsertCartCommandHandler
    {
        public async Task<CartDto> HandleAsync(UpsertCartCommand command, CancellationToken cancellationToken = default)
        {
            Cart cart;
            var existingCart = await cartRepository.GetByUserIdAsync(command.UserId);

            if (existingCart is null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = command.UserId,
                    UpdatedAt = DateTime.UtcNow,
                    Items = command.Items.Select(item => new CartItem
                    {
                        Id = Guid.NewGuid(),
                        Sku = item.Sku,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };
            }
            else
            {
                cart = new Cart
                {
                    Id = existingCart.Id,
                    UserId = existingCart.UserId,
                    UpdatedAt = DateTime.UtcNow,
                    Items = command.Items.Select(item => new CartItem
                    {
                        Id = Guid.NewGuid(),
                        CartId = existingCart.Id,
                        Sku = item.Sku,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };
            }

            var savedCart = await cartRepository.UpsertAsync(cart, cancellationToken);

            await publish.Publish(new CartUpdatedEvent
            {
                CartId = savedCart.Id,
                UserId = savedCart.UserId,
                Items = savedCart.Items.Select(item => new CartItemEventDto
                {
                    Sku = item.Sku,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            }, cancellationToken);

            var cartDto = CartDto.CreateDto(savedCart);

            return cartDto;
        }
    }
}