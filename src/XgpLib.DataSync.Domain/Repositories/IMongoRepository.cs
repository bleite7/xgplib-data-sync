namespace XgpLib.DataSync.Domain.Repositories;

public interface IMongoRepository<in T> where T : IBaseEntity
{
    Task ReplaceOneAsync(T document, CancellationToken stoppingToken, bool isUpsert = false);
}
