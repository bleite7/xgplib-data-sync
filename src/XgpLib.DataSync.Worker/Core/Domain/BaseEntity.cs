namespace XgpLib.DataSync.Worker.Core.Domain;

public abstract class BaseEntity : IBaseEntity
{
    public long? Id { get; set; }
    public DateTime UpdatedAt { get; set; }

    protected BaseEntity() => UpdatedAt = DateTime.UtcNow;
}
