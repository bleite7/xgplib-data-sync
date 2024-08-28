using IGDB;
using IGDB.Models;
using Microsoft.Extensions.Logging;
using Moq;
using XgpLib.DataSync.Domain.Services;
using XgpLib.DataSync.Infrastructure.Services;

namespace XgpLib.DataSync.Infrastructure.Tests.Services;

public class IgdbDataServiceTests
{
    [Fact]
    public async Task ListAllAsync_ShouldReturnListOfGames()
    {
        // Arrange
        Mock<ILogger<IgdbDataService>> mockLogger = new();
        IIgdbDataService igdbDataService = new IgdbDataService(mockLogger.Object, new IGDBClient(
            Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
            Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")));

        // Act
        List<Game> games = await igdbDataService.ListAllAsync<Game>(
            IGDBClient.Endpoints.Games,
            ["id", "name"],
            "where id = (271033,308051);");

        // Assert
        Assert.NotNull(games);
        Assert.Equal(2, games.Count);
        Assert.Equal("Hell Well", games[0].Name);
        Assert.Equal("Double Dragon Revive", games[1].Name);
    }
}
