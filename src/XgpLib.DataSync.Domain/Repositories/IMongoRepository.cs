namespace XgpLib.DataSync.Domain.Repositories;

public interface IMongoRepository<T> where T : IBaseEntity
{
    Task ReplaceOneAsync(T document, bool IsUpsert = false);
}
