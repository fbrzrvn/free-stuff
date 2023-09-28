using FluentAssertions;
using FreeStuff.Application.Item.Commands.Update;
using FreeStuff.Domain.Item;
using FreeStuff.Domain.User;
using FreeStuff.Infrastructure.Item;
using NSubstitute;

namespace FreeStuff.Application.Tests.Unit.Item;

public class UpdateItemCommandHandlerTests
{
    private readonly UpdateItemCommandHandler _handler;
    private readonly IItemRepository          _itemRepository = Substitute.For<ItemRepository>();

    public UpdateItemCommandHandlerTests() { _handler = new UpdateItemCommandHandler(_itemRepository); }

    [Fact]
    public async Task UpdateAsync_ShouldReturnAItem_WhenCreated()
    {
        // Arrange
        var userId     = UserId.CreateUnique();
        var itemEntity = ItemEntity.Create("item-title", "item-description", "good as new", userId);

        _itemRepository.CreateAsync(itemEntity);

        var command = new UpdateItemCommand(itemEntity.Id.Value, "title", "description", "good as new");

        // Act
        var actual = await _handler.Handle(command, CancellationToken.None);

        // Assert
        actual.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnError_WhenItemIsNotFound()
    {
        // Arrange
        var itemId  = Guid.NewGuid();
        var command = new UpdateItemCommand(itemId, "title", "description", "good as new");

        // Act
        var actual = await _handler.Handle(command, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeTrue();
        actual.FirstError.Description.Should().Be($"Item not found with id: {itemId}");
    }
}
