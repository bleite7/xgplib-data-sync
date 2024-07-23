namespace XgpLib.DataSync.Worker.Core.Domain;

public abstract class Document : IDocument
{
    public long? Id { get; set; }
    public DateTime UpdatedAt { get; set; }

    protected Document() => UpdatedAt = DateTime.UtcNow;
}
