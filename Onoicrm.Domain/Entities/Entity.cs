namespace Onoicrm.Domain.Entities;

public abstract class Entity
{
    public long Id { get; set; }
    public string ClassName =>  GetType().Name;
    public DateTime LastUpdate { get; set; } = DateTime.Now.ToUniversalTime();
    public DateTime LastView { get; set; }
    public long? CreateUserId { get; set; }
    public long? UpdateUserId { get; set; }
    public long Priority { get; set; } = 0;
    public DateTime CreateDate { get; set; } = DateTime.Now.ToUniversalTime();
    public int ClassId { get=>GetClassId();set{} }
    public int StateId { get; set; } = StateNames.Created;
    protected abstract int GetClassId();
}