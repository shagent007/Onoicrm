namespace Onoicrm.Domain.Entities;

public class ServiceGroup : Entity
{
    public long? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
    public List<ServiceGroupLink> Links { get; set; } = new();
    public string Caption { get; set; } = "";
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? TextColor { get; set; }
    public string? BgColor { get; set; }
    public long? ParentId { get; set; }
    protected override int GetClassId() =>  ClassNames.ServiceGroup;
}