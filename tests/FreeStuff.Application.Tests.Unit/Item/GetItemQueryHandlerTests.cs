using FluentAssertions;
using FreeStuff.Application.Item.Queries.Get;
using FreeStuff.Domain.Item;
using FreeStuff.Domain.User;
using FreeStuff.Infrastructure.Item;
using NSubstitute;

namespace FreeStuff.Application.Tests.Unit.Item;

public class GetItemQueryHandlerTests
{
    private readonly GetItemQueryHandler _handler;
    private readonly IItemRepository     _itemRepository = Substitute.For<ItemRepository>();

    public GetItemQueryHandlerTests() { _handler = new GetItemQueryHandler(_itemRepository); }

    [Fact]
    public async Task GetAsync_ShouldReturnAItem_WhenCreated()
    {
        // Arrange
        var userId     = UserId.CreateUnique();
        var itemEntity = ItemEntity.Create("item-title", "item-description", "good as new", userId);

        _itemRepository.CreateAsync(itemEntity);

        var query = new GetItemQuery(itemEntity.Id.Value);

        // Act
        var actual = await _handler.Handle(query, CancellationToken.None);

        // Assert
        actual.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnError_WhenItemIsNotFound()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var query  = new GetItemQuery(itemId);

        // Act
        var actual = await _handler.Handle(query, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeTrue();
        actual.FirstError.Description.Should().Be($"Item not found with id: {itemId}");
    }
}
