namespace RetailPlatform.Core.Carts.Commands.Delete
{
    public interface IDeleteCartItemCommandHandler
    {
        Task HandleAsync(DeleteCartItemCommand command, CancellationToken cancellationToken = default);
    }
}
