using System.Net;
using FluentAssertions;

namespace FreeStuff.Api.Tests.Integration.Controllers.Items;

public class GetAllEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetAllEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnsOk_WhenItemsExist()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "items",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_ShouldReturnsOk_WhenItemsDoNotExist()
    {
        // Act
        var response = await _httpClient.GetAsync(
            "items",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
