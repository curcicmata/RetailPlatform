using RetailPlatform.Core.DTOs;

namespace RetailPlatform.Core.Carts.Queries
{
    public interface IGetCartQueryHandler
    {
        Task<CartDto?> HandleAsync(GetCartQuery query, CancellationToken cancellationToken = default);
        Task<List<CartDto>> HandleAsync(CancellationToken cancellationToken = default);
    }
}
