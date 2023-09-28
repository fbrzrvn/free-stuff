using FluentAssertions;
using FreeStuff.Application.Item.Commands.Create;
using FreeStuff.Domain.Item;
using FreeStuff.Infrastructure.Item;
using NSubstitute;

namespace FreeStuff.Application.Tests.Unit.Item;

public class CreateItemCommandHandlerTests
{
    private readonly CreateItemCommandHandler _handler;
    private readonly IItemRepository          _itemRepository = Substitute.For<ItemRepository>();

    public CreateItemCommandHandlerTests() { _handler = new CreateItemCommandHandler(_itemRepository); }

    [Fact]
    public async Task CreateAsync_ShouldReturnAItem_WhenCreated()
    {
        // Arrange
        var command = new CreateItemCommand("title", "description", "good as new");

        // Act
        var actual = await _handler.Handle(command, CancellationToken.None);

        // Assert
        actual.Value.Should().NotBeNull();
    }
}
