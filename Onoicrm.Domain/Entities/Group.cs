namespace Onoicrm.Domain.Entities;

public class Group : Entity
{
    public string Caption { get; set; } = "";
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? TextColor { get; set; }
    public string? BgColor { get; set; }
    public long? ParentId { get; set; }
    public Group? Parent { get; set; }
    public virtual ICollection<Group> Children { get; set; } = new List<Group>();
    protected override int GetClassId() =>  ClassNames.Group;
}

