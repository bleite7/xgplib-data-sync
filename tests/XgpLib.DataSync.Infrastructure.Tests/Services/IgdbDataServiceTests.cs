using IGDB;
using IGDB.Models;
using Moq;
using XgpLib.DataSync.Domain.Services;

namespace XgpLib.DataSync.Infrastructure.Tests.Services;

public class IgdbDataServiceTests
{
    [Fact]
    public async Task ListAllAsync_ShouldReturnListOfGames()
    {
        // Arrange
        Mock<IIgdbDataService> mockIgdbDataService = new();
        List<Game> expectedGames =
        [
            new Game { Id = 1, Name = "Game 1" },
            new Game { Id = 2, Name = "Game 2" }
        ];
        mockIgdbDataService.Setup(service => service.ListAllAsync<Game>(
            IGDBClient.Endpoints.Games,
            new string[] { "id", "name" },
            ""))
            .ReturnsAsync(expectedGames);

        // Act
        IIgdbDataService igdbDataService = mockIgdbDataService.Object;
        List<Game> games = await igdbDataService.ListAllAsync<Game>(
            IGDBClient.Endpoints.Games,
            new string[] { "id", "name" },
            "");

        // Assert
        Assert.NotNull(games);
        Assert.Equal(2, games.Count);
        Assert.Equal("Game 1", games[0].Name);
        Assert.Equal("Game 2", games[1].Name);
    }
}
