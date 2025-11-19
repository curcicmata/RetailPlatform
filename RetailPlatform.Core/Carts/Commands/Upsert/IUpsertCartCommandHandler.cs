using RetailPlatform.Core.DTOs;

namespace RetailPlatform.Core.Carts.Commands.Upsert
{
    public interface IUpsertCartCommandHandler
    {
        Task<CartDto> HandleAsync(UpsertCartCommand command, CancellationToken cancellationToken = default);
    }
}
