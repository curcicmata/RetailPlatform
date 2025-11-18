using RetailPlatform.Core.Contracts;
using RetailPlatform.Core.DTOs;

namespace RetailPlatform.Core.Carts.Queries
{
    public class GetCartQueryHandler(ICartRepository repository) : IGetCartQueryHandler
    {
        public async Task<CartDto?> HandleAsync(GetCartQuery query, CancellationToken cancellationToken = default) 
        {
            var cart = await repository.GetByUserIdAsync(query.UserId);

            if (cart == null)
                return null;

            var cartDto = CartDto.CreateDto(cart);

            return cartDto;
        }
    }
}
