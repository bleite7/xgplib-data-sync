namespace XgpLib.DataSync.Worker.Core.Domain;

public class Genre : BaseEntity
{
    public string Name { get; set; }

    public Genre(long? id, string name)
    {
        Id = id;
        Name = name;
    }
}
