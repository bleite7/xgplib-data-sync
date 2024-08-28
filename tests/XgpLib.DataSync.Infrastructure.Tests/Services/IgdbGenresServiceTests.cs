using IGDB;
using IGDB.Models;
using Microsoft.Extensions.Logging;
using Moq;
using XgpLib.DataSync.Domain.Repositories;
using XgpLib.DataSync.Domain.Services;
using XgpLib.DataSync.Infrastructure.Services;

namespace XgpLib.DataSync.Infrastructure.Tests.Services;

public class IgdbGenresServiceTests
{
    [Fact]
    public async Task SyncIgdbGenresAsync_ShouldSyncGenres()
    {
        // Arrange
        Mock<ILogger<IgdbGenresService>> loggerMock = new();
        Mock<IIgdbDataService> igdbDataServiceMock = new();
        Mock<IMongoRepository<Domain.Entities.Genre>> genresRepositoryMock = new();
        List<Genre> genres =
        [
            new() { Id = 1, Name = "Genre 1" },
            new() { Id = 2, Name = "Genre 2" }
        ];

        igdbDataServiceMock.Setup(x => x.ListAllAsync<Genre>(
            IGDBClient.Endpoints.Genres,
            new[] { "id", "name" },
            "")).ReturnsAsync(genres);

        var service = new IgdbGenresService(
            loggerMock.Object,
            igdbDataServiceMock.Object,
            genresRepositoryMock.Object);

        // Act
        await service.SyncIgdbGenresAsync(CancellationToken.None);

        // Assert
        genresRepositoryMock.Verify(x => x
            .ReplaceOneAsync
            (
                It.IsAny<Domain.Entities.Genre>(),
                CancellationToken.None,
                true
            ),
            Times.Exactly(2));
    }
}
