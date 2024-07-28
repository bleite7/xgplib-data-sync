namespace XgpLib.DataSync.Domain.Repositories;

public interface IMongoRepository<T> where T : IBaseEntity
{
    Task ReplaceOneAsync(T document, CancellationToken stoppingToken, bool IsUpsert = false);
}
