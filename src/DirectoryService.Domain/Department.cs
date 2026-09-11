namespace DirectoryService.Domain;

public class Department
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Identifier { get; set; }
    public Guid ParentId { get; set; }
    public string Path { get; set; }
    public short Depth { get; set; }
    public short IsActive { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    public DateTime deletedAt { get; set; }
}
