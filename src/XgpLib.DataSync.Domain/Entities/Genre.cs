namespace XgpLib.DataSync.Domain.Entities;

public class Genre : BaseEntity
{
    public string Name { get; set; }

    public Genre(long? id, string name)
    {
        Id = id;
        Name = name;
    }
}
