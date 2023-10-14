using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FreeStuff.Contracts.Items.Requests;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Api.Tests.Integration.Controllers.Items;

public class CreateEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    public CreateEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Fact]
    public async Task Create_ShouldCreateItem_WhenValidRequestIsSent()
    {
        // Arrange
        var createItemRequest = new CreateItemRequest(
            Constants.Item.Title,
            Constants.Item.Description,
            Constants.Item.Condition,
            Constants.Item.UserId
        );

        // Act
        var response = await _httpClient.PostAsJsonAsync(
            "items",
            createItemRequest,
            CancellationToken.None
        );

        // Assert
        var itemResponse = await response.Content.ReadFromJsonAsync<ItemDto>();

        itemResponse.Should().NotBeNull().And.BeEquivalentTo(createItemRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"http://localhost/items/{itemResponse!.Id}");
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenInvalidRequestIsSent()
    {
        // Arrange
        var createItemRequest = new CreateItemRequest(
            Constants.Item.Title,
            Constants.Item.Description,
            "Old but Gold",
            Constants.Item.UserId
        );

        // Act
        var response = await _httpClient.PostAsJsonAsync(
            "items",
            createItemRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
