namespace RetailPlatform.Domain.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<CartItem> Items { get; set; } = [];
    }
}
