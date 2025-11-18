namespace RetailPlatform.Core.DTOs
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CartItemDto> Items { get; set; } = [];

        public static CartDto CreateDto(Domain.Models.Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                UpdatedAt = cart.UpdatedAt,
                Items = cart.Items.Select(item => new CartItemDto
                {
                    Id = item.Id,
                    Sku = item.Sku,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };
        }
    }
}
