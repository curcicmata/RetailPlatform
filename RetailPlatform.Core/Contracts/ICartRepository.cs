using RetailPlatform.Domain.Models;

namespace RetailPlatform.Core.Contracts
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task<Cart?> GetByIdAsync(Guid id);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<Cart> UpsertAsync(Cart cart, CancellationToken cancellationToken = default);
    }
}
