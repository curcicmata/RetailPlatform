namespace RetailPlatform.Contracts
{
    public class CartUpdatedEvent
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CartItemEventDto> Items { get; set; } = [];
    }
}
