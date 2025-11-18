namespace RetailPlatform.Contracts
{
    public class CartItemEventDto
    {
        public string Sku { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
