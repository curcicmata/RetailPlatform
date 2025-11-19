using MassTransit;
using Microsoft.EntityFrameworkCore;
using RetailPlatform.Core.Contracts;
using RetailPlatform.Domain.Models;

namespace RetailPlatform.Persistence.Repositories
{
    public class CartRepository(ApplicationDbContext dbContext) : ICartRepository
    {
        public async Task AddAsync(Cart cart)
        {
            await dbContext.Carts.AddAsync(cart);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cart = await dbContext.Carts
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (cart is not null)
            {
                dbContext.Carts.Remove(cart);
            }
        }

        public async Task<Cart?> GetByIdAsync(Guid id)
        {
            return await dbContext.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Cart?> GetByUserIdAsync(Guid userId)
        {
            return dbContext.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(Cart cart)
        {
            dbContext.Carts.Update(cart);
            return Task.CompletedTask;
        }

        public async Task<Cart> UpsertAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            var existing = await GetByUserIdAsync(cart.UserId);
            if (existing is null)
            {
                await AddAsync(cart);
                await SaveChangesAsync(cancellationToken);

                return cart;
            }

            // Update existing cart
            existing.UpdatedAt = DateTime.UtcNow;

            dbContext.CartItems.RemoveRange(existing.Items);

            foreach (var item in cart.Items)
            {
                item.CartId = existing.Id;
                dbContext.CartItems.Add(item);
            }

            await UpdateAsync(existing);
            await SaveChangesAsync(cancellationToken);

            return existing;
        }
    }
}
