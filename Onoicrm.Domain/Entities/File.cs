namespace Onoicrm.Domain.Entities;

public class AttachedFile : Entity
{
    public string? StorageId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public long? FileSize { get; set; }
    public string Extension { get; set; }
    public string Base64 { get; set; } = "";
    protected override int GetClassId() => ClassNames.File;
}