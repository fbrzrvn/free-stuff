using FluentAssertions;
using FreeStuff.Application.Item.Queries.GetAll;
using FreeStuff.Domain.Item;
using NSubstitute;

namespace FreeStuff.Application.Tests.Unit.Item;

public class GetAllItemsQueryHandlerTests
{
    private readonly GetAllItemsQueryHandler _handler;
    private readonly IItemRepository  _itemRepository = Substitute.For<IItemRepository>();

    public GetAllItemsQueryHandlerTests()
    {
        _handler = new GetAllItemsQueryHandler(_itemRepository);
    }

    [Fact]
    public async Task GetAllItemQueryHandler_ShouldReturnItemsList_WhenCalled()
    {
        // Arrange
        var query = new GetAllItemsQuery();

        // Act
        var actual = await _handler.Handle(query, CancellationToken.None);

        // Assert
        actual.Should().NotBeNull();
    }
}
