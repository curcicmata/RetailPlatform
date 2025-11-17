using RetailPlatform.Domain.Interfaces;

namespace RetailPlatform.Domain.Models
{
    public class Cart : ISoftDeletable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<CartItem> Items { get; set; } = [];
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOnUtc { get; set; }
    }
}
