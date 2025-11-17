using RetailPlatform.Domain.Interfaces;

namespace RetailPlatform.Domain.Models
{
    public class CartItem : ISoftDeletable
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public string Sku { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Cart Cart { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOnUtc { get; set; }
    }
}
