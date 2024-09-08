using IGDB;
using IGDB.Models;
using Microsoft.Extensions.Logging;
using Moq;
using XgpLib.DataSync.Domain.Repositories;
using XgpLib.DataSync.Domain.Services;
using XgpLib.DataSync.Infrastructure.Services;

namespace XgpLib.DataSync.Infrastructure.Tests.Services;

public class IgdbPlatformsServiceTests
{
    private static readonly string[] platformsFields = ["id", "name"];

    [Fact]
    public async Task SyncIgdbPlatformsAsync_ShouldSyncGenres()
    {
        // Arrange
        Mock<ILogger<IgdbPlatformsService>> loggerMock = new();
        Mock<IIgdbDataService> igdbDataServiceMock = new();
        Mock<IMongoRepository<Domain.Entities.Platform>> platformsRepositoryMock = new();
        List<Platform> platforms =
        [
            new() { Id = 1, Name = "Platform 1" },
            new() { Id = 2, Name = "Platform 2" }
        ];

        igdbDataServiceMock.Setup(x => x.ListAllAsync<Platform>(
            IGDBClient.Endpoints.Platforms,
            platformsFields,
            "")).ReturnsAsync(platforms);

        var service = new IgdbPlatformsService(
            loggerMock.Object,
            igdbDataServiceMock.Object,
            platformsRepositoryMock.Object);

        // Act
        await service.SyncIgdbPlatformsAsync(CancellationToken.None);

        // Assert
        platformsRepositoryMock.Verify(x => x
            .ReplaceOneAsync
            (
                It.IsAny<Domain.Entities.Platform>(),
                CancellationToken.None,
                true
            ),
            Times.Exactly(2));
    }
}
