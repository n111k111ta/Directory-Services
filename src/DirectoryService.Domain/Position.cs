namespace DirectoryService.Domain;


public class Position
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}