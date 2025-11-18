using RetailPlatform.Core.DTOs;

namespace RetailPlatform.Core.Carts.Commands
{
    public class UpsertCartCommand
    {
        public Guid UserId { get; set; }
        public List<CartItemDto> Items { get; set; }
    }
}
