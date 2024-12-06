namespace Onoicrm.Domain.Entities;

public class ServiceGroup : Group
{
    public long? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
    public List<ServiceGroupLink> Links { get; set; } = new();
    
    protected override int GetClassId() =>  ClassNames.ServiceGroup;
}