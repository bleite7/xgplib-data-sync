namespace XgpLib.DataSync.Worker.Core.Domain;

public interface IBaseEntity
{
    public long? Id { get; set; }
    public DateTime UpdatedAt { get; set; }
}
