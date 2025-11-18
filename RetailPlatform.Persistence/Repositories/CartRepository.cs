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

        public async Task DeleteAsync(Guid id)
        {
            var cart = await dbContext.Carts.FindAsync(id);

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

            if (existing == null)
            {
                // Create new cart
                cart.Id = Guid.NewGuid();
                cart.UpdatedAt = DateTime.UtcNow;

                await AddAsync(cart);
                await SaveChangesAsync(cancellationToken);

                return cart;
            }

            // Update existing cart
            existing.UpdatedAt = DateTime.UtcNow;

            // Replace items (simple approach)
            existing.Items.Clear();
            foreach (var item in cart.Items)
            {
                existing.Items.Add(new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = existing.Id,
                    Sku = item.Sku,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            await UpdateAsync(existing);
            await SaveChangesAsync(cancellationToken);

            return existing;
        }
    }
}
