namespace Onoicrm.Domain.Entities;

public class Tooth : Entity
{
    public string Caption { get; set; } = "";
    public string Description { get; set; } = "";
    public int Position { get; set; }
    public int Quarter { get; set; }
    public ICollection<Channel> Channels { get; set; } = new List<Channel>();
    protected override int GetClassId() => ClassNames.Tooth;
}