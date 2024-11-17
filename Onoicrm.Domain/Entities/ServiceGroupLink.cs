namespace Onoicrm.Domain.Entities;

public class ServiceGroupLink : Entity
{
    public long ServiceId { get; set; }
    public long ServiceGroupId { get; set; }
    protected override int GetClassId() =>  ClassNames.ServiceGroupLink;
}