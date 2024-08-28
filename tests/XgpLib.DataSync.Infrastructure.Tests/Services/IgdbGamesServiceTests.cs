using IGDB;
using IGDB.Models;
using Microsoft.Extensions.Logging;
using Moq;
using XgpLib.DataSync.Domain.Repositories;
using XgpLib.DataSync.Domain.Services;
using XgpLib.DataSync.Infrastructure.Services;

namespace XgpLib.DataSync.Infrastructure.Tests.Services;

public class IgdbGamesServiceTests
{
    [Fact]
    public async Task SyncIgdbGamesByPlatformAsync_ShouldSyncGamesForPlatform()
    {
        // Arrange
        var platformId = 169;
        Mock<ILogger<IgdbGamesService>> loggerMock = new();
        Mock<IIgdbDataService> igdbDataServiceMock = new();
        Mock<IMongoRepository<Domain.Entities.Game>> gamesRepositoryMock = new();
        List<Game> games =
        [
            new() { Id = 1, Name = "Game 1" },
            new() { Id = 2, Name = "Game 2" }
        ];

        igdbDataServiceMock.Setup(x => x.ListAllAsync<Game>(
            IGDBClient.Endpoints.Games,
            new[] { "id", "name" },
            $"where platforms = [{platformId}];")).ReturnsAsync(games);

        var service = new IgdbGamesService(
            loggerMock.Object,
            igdbDataServiceMock.Object,
            gamesRepositoryMock.Object);

        // Act
        await service.SyncIgdbGamesByPlatformAsync(platformId, CancellationToken.None);

        // Assert
        gamesRepositoryMock.Verify(x => x
            .ReplaceOneAsync
            (
                It.IsAny<Domain.Entities.Game>(),
                CancellationToken.None,
                true
            ),
            Times.Exactly(2));
    }
}
