namespace RetailPlatform.Core.Carts.Commands.Delete
{
    public interface IDeleteCartCommandHandler
    {
        Task HandleAsync(DeleteCartCommand command, CancellationToken cancellationToken = default);
    }
}
