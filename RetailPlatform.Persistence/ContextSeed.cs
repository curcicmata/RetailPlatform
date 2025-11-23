using RetailPlatform.Domain.Models;

namespace RetailPlatform.Persistence
{
    public static class ContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (dbContext.Carts.Any() == false && dbContext.CartItems.Any() == false)
            {
                var cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    UpdatedAt = DateTime.UtcNow,
                };

                var cartItem = new List<CartItem>
                {
                    new() {
                        Id = Guid.NewGuid(),
                        CartId = cart.Id,
                        Sku = "SKU-NTB-LENOVO-A5-16-1TB-BLUE",
                        Quantity = 10,
                        Price = 599.99m
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        CartId = cart.Id,
                        Sku = "SKU-APPL-IPH-16-128-BLACK",
                        Quantity = 5,
                        Price = 1299.99m
                    }
                };


                await dbContext.Carts.AddAsync(cart);
                await dbContext.CartItems.AddRangeAsync(cartItem);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
