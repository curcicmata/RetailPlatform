namespace RetailPlatform.Core.DTOs
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public static CartItemDto CreateDto(Domain.Models.CartItem cartItem)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                Sku = cartItem.Sku,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            };
        }
    }
}
