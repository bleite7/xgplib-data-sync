namespace XgpLib.DataSync.Worker.Core.Domain.Services;

public interface IIgdbDataService
{
    Task<List<T>> ListAllAsync<T>(
        string endpoint,
        string[] fields,
        string additionalQuery = "") where T : class;
}
