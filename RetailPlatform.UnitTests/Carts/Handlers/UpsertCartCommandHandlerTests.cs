using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using RetailPlatform.Contracts;
using RetailPlatform.Core.Carts.Commands.Upsert;
using RetailPlatform.Core.Contracts;
using RetailPlatform.Core.DTOs;
using RetailPlatform.Domain.Models;

namespace RetailPlatform.UnitTests.Carts.Handlers
{
    public class UpsertCartCommandHandlerTests
    {
        private readonly Mock<ICartRepository> _repoMock = new();
        private readonly Mock<IPublishEndpoint> _publishMock = new();
        private readonly Mock<ILogger<UpsertCartCommandHandler>> _loggerMock = new();

        [Fact]
        public async Task HandleAsync_ShouldCreateNewCart_WhenCartDoesNotExist()
        {
            // ARRANGE
            var userId = Guid.NewGuid();

            _repoMock.Setup(r => r.GetByUserIdAsync(userId))
                     .ReturnsAsync((Cart?)null);

            _repoMock.Setup(r => r.UpsertAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Cart cart, CancellationToken ct) => cart);

            var handler = new UpsertCartCommandHandler(
                _repoMock.Object,
                _publishMock.Object,
                _loggerMock.Object
            );

            var command = new UpsertCartCommand
            {
                UserId = userId,
                Items = new List<CartItemDto>
                {
                    new() { Sku = "SKU-NOTEBOOK-A5", Quantity = 1, Price = 10 }
                }
            };

            // ACT
            var result = await handler.HandleAsync(command, CancellationToken.None);

            // ASSERT
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.Items.Should().HaveCount(1);

            _repoMock.Verify(r => r.UpsertAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
            _publishMock.Verify(p => p.Publish(It.IsAny<CartUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ShouldUpdateExistingCart()
        {
            // ARRANGE
            var userId = Guid.NewGuid();
            var existingCart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Items = new List<CartItem>()
            };

            _repoMock.Setup(r => r.GetByUserIdAsync(userId))
                     .ReturnsAsync(existingCart);

            _repoMock.Setup(r => r.UpsertAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((Cart c, CancellationToken ct) => c);


            var handler = new UpsertCartCommandHandler(
                _repoMock.Object,
                _publishMock.Object,
                _loggerMock.Object
            );

            var command = new UpsertCartCommand
            {
                UserId = userId,
                Items = new List<CartItemDto>
                {
                    new() { Sku = "SKU-NOTEBOOK-A5", Quantity = 2, Price = 20 }
                }
            };

            // ACT
            var result = await handler.HandleAsync(command, CancellationToken.None);

            // ASSERT
            result.Items.Should().HaveCount(1);
            result.Items[0].Sku.Should().Be("SKU-NOTEBOOK-A5");
            result.Items[0].Quantity.Should().Be(2);


            _repoMock.Verify(r => r.UpsertAsync(
                It.Is<Cart>(c =>
                    c.Id == existingCart.Id &&
                    c.UserId == existingCart.UserId &&
                    c.Items.Count == 1 &&
                    c.Items[0].Sku == "SKU-NOTEBOOK-A5"
                ),
                It.IsAny<CancellationToken>()),
                Times.Once);
            _publishMock.Verify(p => p.Publish(It.IsAny<CartUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
