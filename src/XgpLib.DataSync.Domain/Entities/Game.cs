namespace XgpLib.DataSync.Domain.Entities;

public class Platform : BaseEntity
{
    public string Name { get; set; }

    public Platform(long? id, string name)
    {
        Id = id;
        Name = name;
    }
}
