using Moq;
using XgpLib.DataSync.Application.UseCases;
using XgpLib.DataSync.Domain.Entities;
using XgpLib.DataSync.Domain.Services;

namespace XgpLib.DataSync.Application.UnitTests;

public class SyncDataTests
{
    private const long _xboxSeriesPlatformId = 169;

    [Fact]
    public async Task SyncIgdbDataAsync()
    {
        // Arrange
        Mock<IIgdbGamesService> mockIgdbGamesService = new Mock<IIgdbGamesService>();
        Mock<IIgdbGenresService> mockIgdbGenresService = new Mock<IIgdbGenresService>();
        Mock<IIgdbPlatformsService> mockIgdbPlatformsService = new Mock<IIgdbPlatformsService>();

        SyncData syncData = new(
            mockIgdbGamesService.Object,
            mockIgdbGenresService.Object,
            mockIgdbPlatformsService.Object);

        // Act
        await syncData.SyncIgdbDataAsync(CancellationToken.None);

        // Assert
        mockIgdbGamesService.Verify(x => x.SyncIgdbGamesByPlatformAsync(_xboxSeriesPlatformId, It.IsAny<CancellationToken>()), Times.Once);
        mockIgdbGenresService.Verify(x => x.SyncIgdbGenresAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockIgdbPlatformsService.Verify(x => x.SyncIgdbPlatformsAsync(It.IsAny<CancellationToken>()), Times.Once);

    }

}
