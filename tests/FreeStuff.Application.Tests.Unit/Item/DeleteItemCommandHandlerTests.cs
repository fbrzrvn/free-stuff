using FluentAssertions;
using FreeStuff.Application.Item.Commands.Delete;
using FreeStuff.Domain.Item;
using FreeStuff.Domain.User;
using FreeStuff.Infrastructure.Item;
using NSubstitute;

namespace FreeStuff.Application.Tests.Unit.Item;

public class DeleteItemCommandHandlerTests
{
    private readonly DeleteItemCommandHandler _handler;
    private readonly IItemRepository          _itemRepository = Substitute.For<ItemRepository>();

    public DeleteItemCommandHandlerTests() { _handler = new DeleteItemCommandHandler(_itemRepository); }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenDeleted()
    {
        // Arrange
        var userId     = UserId.CreateUnique();
        var itemEntity = ItemEntity.Create("item-title", "item-description", "good as new", userId);

        _itemRepository.CreateAsync(itemEntity);

        var command = new DeleteItemCommand(Guid.Parse(itemEntity.Id.Value.ToString()));

        // Act
        var actual = await _handler.Handle(command, CancellationToken.None);

        // Assert
        actual.Value.Should().Be(true);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnError_WhenItemIsNotFound()
    {
        // Arrange
        var itemId  = Guid.NewGuid();
        var command = new DeleteItemCommand(itemId);

        // Act
        var actual = await _handler.Handle(command, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeTrue();
        actual.FirstError.Description.Should().Be($"Item not found with id: {itemId}");
    }
}
