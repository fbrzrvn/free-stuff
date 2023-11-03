using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FreeStuff.Contracts.Items.Requests;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Api.Tests.Integration.Controllers.Items;

public class GetEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Fact]
    public async Task Get_ShouldReturnsOk_WhenItemExists()
    {
        // Arrange
        var createItemRequest = new CreateItemRequest(
            Constants.Item.Title,
            Constants.Item.Description,
            Constants.Item.CategoryName,
            Constants.Item.Condition,
            Constants.Item.UserId
        );
        var createdResponse = await _httpClient.PostAsJsonAsync(
            "items",
            createItemRequest,
            CancellationToken.None
        );
        var item = await createdResponse.Content.ReadFromJsonAsync<ItemDto>();

        // Act
        var response = await _httpClient.GetAsync($"items/{item!.Id}", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_ShouldReturnsNotFound_WhenItemDoesNotExist()
    {
        // Act
        var response = await _httpClient.DeleteAsync($"items/{Guid.NewGuid()}", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
