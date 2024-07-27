namespace XgpLib.DataSync.Worker.Core.Domain;

public class Game : BaseEntity
{
    public string Name { get; set; }

    public Game(long? id, string name)
    {
        Id = id;
        Name = name;
    }
}
