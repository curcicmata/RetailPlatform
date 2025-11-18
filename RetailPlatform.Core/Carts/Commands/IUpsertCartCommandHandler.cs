using RetailPlatform.Core.DTOs;

namespace RetailPlatform.Core.Carts.Commands
{
    public interface IUpsertCartCommandHandler
    {
        Task<CartDto> HandleAsync(UpsertCartCommand command, CancellationToken cancellationToken = default);
    }
}
