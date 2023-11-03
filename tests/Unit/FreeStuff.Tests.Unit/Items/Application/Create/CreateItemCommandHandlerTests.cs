using FluentAssertions;
using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Items.Application.Create;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Items.Domain;
using FreeStuff.Items.Domain.Ports;
using FreeStuff.Tests.Unit.Items.TestUtils;
using MapsterMapper;
using NSubstitute;

namespace FreeStuff.Tests.Unit.Items.Application.Create;

public class CreateItemCommandHandlerTests
{
    private readonly CreateItemCommandHandler _handler;
    private readonly ICategoryRepository      _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IItemRepository          _itemRepository     = Substitute.For<IItemRepository>();
    private readonly IMapper                  _mapper             = Substitute.For<IMapper>();

    public CreateItemCommandHandlerTests()
    {
        _handler = new CreateItemCommandHandler(
            _categoryRepository,
            _itemRepository,
            _mapper
        );
    }

    [Fact]
    public async Task HandleCreateItemCommand_ShouldCreateAndReturnItem_WhenItemIsValid()
    {
        // Arrange
        var createItemCommand = ItemCommandUtils.NewCreateItemCommand();

        _categoryRepository.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Category.Create("Other"));
        _itemRepository.CreateAsync(Arg.Any<Item>(), Arg.Any<CancellationToken>())
                       .ReturnsForAnyArgs(Task.FromResult(ItemUtils.CreateItem()));

        _mapper.Map<ItemDto>(Arg.Any<Item>())
               .Returns(ItemUtils.CreateItemDto());

        // Act
        var actual = await _handler.Handle(createItemCommand, CancellationToken.None);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Id.Should().NotBeEmpty();
        actual.Value.Title.Should().Be(createItemCommand.Title);
        actual.Value.Description.Should().Be(createItemCommand.Description);
        actual.Value.CategoryName.Should().Be(createItemCommand.CategoryName);
        actual.Value.Condition.Should().Be(createItemCommand.Condition);
        actual.Value.UserId.Should().Be(createItemCommand.UserId);

        await _itemRepository.Received(1).CreateAsync(Arg.Any<Item>(), CancellationToken.None);
        await _itemRepository.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
