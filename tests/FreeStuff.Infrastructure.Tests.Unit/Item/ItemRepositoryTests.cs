using FluentAssertions;
using FreeStuff.Domain.Item;
using FreeStuff.Domain.User;
using FreeStuff.Infrastructure.Item;
using NSubstitute;

namespace FreeStuff.Infrastructure.Tests.Unit.Item;

public class ItemRepositoryTests
{
    private readonly IItemRepository _itemRepository = Substitute.For<ItemRepository>();

    [Fact]
    public void CreateAsync_ShouldReturnTrue_WhenCreated()
    {
        // Arrange
        var userId = UserId.CreateUnique();
        var item   = ItemEntity.Create("item title", "item description", "good as new", userId);

        // Act
        var actual = _itemRepository.CreateAsync(item);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Get_ShouldReturnItem_WhenFound()
    {
        // Arrange
        var userId = UserId.CreateUnique();
        var item   = ItemEntity.Create("item title", "item description", "good as new", userId);

        _itemRepository.CreateAsync(item);

        // Act
        var actual = _itemRepository.GetAsync(Guid.Parse(item.Id.Value.ToString()));

        // Assert
        actual.Should().NotBeNull();
    }

    [Fact]
    public void GetByTitle_ShouldReturnItem_WhenFound()
    {
        // Arrange
        var userId = UserId.CreateUnique();
        var item   = ItemEntity.Create("item title", "item description", "good as new", userId);

        _itemRepository.CreateAsync(item);

        // Act
        var actual = _itemRepository.GetByTitleAsync("item-title");

        // Assert
        actual?.Title.Should().Be("item-title");
        actual?.Description.Should().Be("item-description");
        actual?.Condition.Should().Be("good as new");
    }

    [Fact]
    public void GetAll_ShouldReturnAnEmptyList_WhenThereAreNoItems()
    {
        // Act
        var actual = _itemRepository.GetAllAsync();

        // Assert
        actual.Should().HaveCount(0);
    }

    [Fact]
    public void GetAll_ShouldReturnItemsList_WhenThereAreItems()
    {
        // Arrange
        var item = ItemEntity.Create(
            "item-title",
            "item-description",
            "good as new",
            UserId.CreateUnique()
        );

        _itemRepository.CreateAsync(item);

        // Act
        var actual = _itemRepository.GetAllAsync();

        // Assert
        actual.Should().HaveCount(1);
    }

    [Fact]
    public void DeleteAsync_ShouldReturnTrue_WhenDeleted()
    {
        // Arrange
        var userId = UserId.CreateUnique();
        var item   = ItemEntity.Create("item title", "item description", "good as new", userId);

        _itemRepository.CreateAsync(item);

        // Act
        var actual = _itemRepository.DeleteAsync(Guid.Parse(item.Id.Value.ToString()));

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void DeleteAsync_ShouldReturnFalse_WhenItemIsNotDeleted()
    {
        // Act
        var actual = _itemRepository.DeleteAsync(Guid.NewGuid());

        // Assert
        actual.Should().BeFalse();
    }
}
