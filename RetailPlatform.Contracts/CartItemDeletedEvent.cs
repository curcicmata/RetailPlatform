namespace RetailPlatform.Contracts
{
    public class CartItemDeletedEvent
    {
        public Guid CartId { get; set; }
        public Guid ItemId { get; set; }
    }
}
