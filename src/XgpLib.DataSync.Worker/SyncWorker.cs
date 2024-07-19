using IGDB;
using IGDB.Models;
using XgpLib.DataSync.Worker.Core.Domain.Services;

namespace XgpLib.DataSync.Worker;

public class SyncWorker(
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<SyncWorker> logger,
    IIgdbService igdbService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var genres = await igdbService.ListAll<Genre>(IGDBClient.Endpoints.Genres, ["id", "name"]);
        foreach (var genre in genres)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Genre {id} {name}", genre.Id, genre.Name);
            }
        }
        // When completed, the entire app host will stop.
        hostApplicationLifetime.StopApplication();
    }
}
