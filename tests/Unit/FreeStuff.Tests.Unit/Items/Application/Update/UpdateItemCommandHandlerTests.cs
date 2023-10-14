using FluentAssertions;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Application.Shared.Mapping;
using FreeStuff.Items.Application.Update;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Items.Domain.ValueObjects;
using FreeStuff.Tests.Unit.Items.TestUtils;
using FreeStuff.Tests.Utils.Constants;
using FreeStuff.Tests.Utils.Extensions;
using MapsterMapper;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Items.Application.Update;

public class UpdateItemCommandHandlerTests
{
    private readonly UpdateItemCommandHandler _handler;
    private readonly IItemRepository          _itemRepository = Substitute.For<IItemRepository>();
    private readonly IMapper                  _mapper         = Substitute.For<IMapper>();

    public UpdateItemCommandHandlerTests()
    {
        _handler = new UpdateItemCommandHandler(_itemRepository, _mapper);
    }

    [Fact]
    public async Task HandleUpdateItemCommandHandler_ShouldUpdateItem_WhenFound()
    {
        // Arrange
        var updateItemCommand = ItemCommandUtils.NewUpdateItemCommand();
        var item              = ItemUtils.CreateItem();

        item.Update(
            Constants.Item.EditedTitle,
            Constants.Item.EditedDescription,
            Constants.Item.EditedCondition.MapStringToItemCondition()
        );

        var expected = item.MapItemToDto();

        _itemRepository
            .GetAsync(Arg.Any<ItemId>(), Arg.Any<CancellationToken>())
            .Returns(item);

        _mapper.Map<ItemDto>(Arg.Any<Item>())
               .Returns(expected);

        // Act
        var actual = await _handler.Handle(updateItemCommand, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeEquivalentTo(expected);

        await _itemRepository.Received(1).GetAsync(Arg.Any<ItemId>(), Arg.Any<CancellationToken>());
        _itemRepository.Received(1).Update(item);
        await _itemRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleUpdateItemCommandHandler_ShouldReturnNotFoundError_WhenNotFound()
    {
        // Arrange
        var updateItemCommand = ItemCommandUtils.NewUpdateItemCommand();

        _itemRepository
            .GetAsync(Arg.Any<ItemId>(), Arg.Any<CancellationToken>())
            .Returns((Item)null!);

        // Act
        var actual = await _handler.Handle(updateItemCommand, CancellationToken.None);

        // Assert
        actual.ValidateNotFoundError(updateItemCommand.Id);

        await _itemRepository.Received(1).GetAsync(Arg.Any<ItemId>(), Arg.Any<CancellationToken>());
    }
}
