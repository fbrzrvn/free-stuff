using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FreeStuff.Contracts.Categories.Requests;
using FreeStuff.Contracts.Categories.Responses;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Api.Tests.Integration.Controllers.Categories;

public class UpdateEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    public UpdateEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Fact]
    public async Task Update_ShouldUpdateCategory_WhenFoundAndValidRequestIsSent()
    {
        // Arrange
        var updateCategoryRequest = new UpdateCategoryRequest(Constants.Category.Test, Constants.Category.EditedName);

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            "categories",
            updateCategoryRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<CategoryResponse>();
        actual?.Name.Should().Be(Constants.Category.EditedName);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenInvalidRequestIsSent()
    {
        // Arrange
        var updateCategoryRequest = new UpdateCategoryRequest(Constants.Category.Test, string.Empty);

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            "categories",
            updateCategoryRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_ShouldReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var updateCategoryRequest = new UpdateCategoryRequest(Constants.Category.Name, Constants.Category.EditedName);

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            "categories",
            updateCategoryRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
