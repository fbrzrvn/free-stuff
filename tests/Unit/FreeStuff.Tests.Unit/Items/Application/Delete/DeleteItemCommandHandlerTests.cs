using FluentAssertions;
using FreeStuff.Items.Application.Delete;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using FreeStuff.Tests.Unit.Items.TestUtils;
using FreeStuff.Tests.Utils.Extensions;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Items.Application.Delete;

public class DeleteItemCommandHandlerTests
{
    private readonly DeleteItemCommandHandler _handler;
    private readonly IItemRepository          _itemRepository = Substitute.For<IItemRepository>();

    public DeleteItemCommandHandlerTests()
    {
        _handler = new DeleteItemCommandHandler(_itemRepository);
    }

    [Fact]
    public async Task HandlerDeleteItemCommandHandler_ShouldDeleteItemAndReturnTrue_WhenFound()
    {
        // Arrange
        var item              = ItemUtils.CreateItem();
        var deleteItemCommand = new DeleteItemCommand(Guid.Parse(item.Id.Value.ToString()));

        _itemRepository
            .GetAsync(Arg.Any<ItemId>(), Arg.Any<CancellationToken>())
            .Returns(item);

        // Act
        var actual = await _handler.Handle(deleteItemCommand, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeTrue();

        _itemRepository.Received(1).Delete(item);
        await _itemRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandlerDeleteItemCommandHandler_ShouldReturnNotFoundError_WhenNotFound()
    {
        // Arrange
        var item              = ItemUtils.CreateItem();
        var deleteItemCommand = new DeleteItemCommand(Guid.Parse(item.Id.Value.ToString()));

        // Act
        var actual = await _handler.Handle(deleteItemCommand, CancellationToken.None);

        // Assert
        actual.ValidateNotFoundError(item.Id.Value);

        _itemRepository.Received(0).Delete(item);
        await _itemRepository.Received(0).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
