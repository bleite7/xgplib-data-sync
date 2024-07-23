namespace XgpLib.DataSync.Worker.Core.Domain;

public interface IDocument
{
    public long? Id { get; set; }
    public DateTime UpdatedAt { get; set; }
}
