namespace XgpLib.DataSync.Domain.Entities;

public class Game : BaseEntity
{
    public string Name { get; set; }

    public Game(long? id, string name)
    {
        Id = id;
        Name = name;
    }
}
