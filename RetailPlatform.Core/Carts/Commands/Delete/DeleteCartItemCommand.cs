namespace RetailPlatform.Core.Carts.Commands.Delete
{
    public class DeleteCartItemCommand
    {
        public Guid CartId { get; set; }
        public Guid ItemId { get; set; }
    }
}
