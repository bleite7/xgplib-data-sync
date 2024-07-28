namespace XgpLib.DataSync.Domain.Common;

public interface IBaseEntity
{
    public long? Id { get; set; }
    public DateTime UpdatedAt { get; set; }
}
