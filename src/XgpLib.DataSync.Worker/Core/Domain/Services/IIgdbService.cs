using IGDB.Models;

namespace XgpLib.DataSync.Worker.Core.Domain.Services;

public interface IIgdbService
{
    Task<List<Genre>> ListAllGenres();
}
