namespace Onoicrm.Domain.Entities;

public class Channel : Entity
{
    public string Caption { get; set; }
    public string? Description { get; set; }
    public long ToothId { get; set; }
    public Tooth? Tooth { get; set; }
    protected override int GetClassId() => ClassNames.Channel;
}


public class InformationSource : Entity
{
    public string Caption { get; set; }
    public string? Description { get; set; }
    protected override int GetClassId() => ClassNames.Channel;
}