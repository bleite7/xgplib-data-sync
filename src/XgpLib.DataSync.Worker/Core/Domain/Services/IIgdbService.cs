using IGDB.Models;

namespace XgpLib.DataSync.Worker.Core.Domain.Services;

public interface IIgdbService
{
    Task<List<T>> ListAll<T>(
        string endpoint,
        string[] fields,
        string additionalQuery = "") where T : class;
}
