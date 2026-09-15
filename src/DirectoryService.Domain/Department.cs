using DirectoryService.Domain.valueObject;

namespace DirectoryService.Domain;

public class Department
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Identifier { get; private set; }
    public Guid? ParentId { get; private set; }
    public string Path { get; private set; }
    public short Depth { get; private set; }
    public EntityLifeTime EntityLifeTime { get; private set; }
    private Department(Guid id, string name, string identifier, Guid? parentId, string path, short depth, EntityLifeTime entityLifeTime1)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        EntityLifeTime = entityLifeTime1;
    }
    public static Department Create(Guid id, string name, string identifier, Guid parentId, string path, short depth, EntityLifeTime entityLifeTime1)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Пустой идентификатор!");
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException("Пустое имя!");
        }
        if (string.IsNullOrWhiteSpace(identifier))
        {
            throw new ArgumentNullException("Пустой идентификатор!");
        }
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException("Пустой путь!");
        }
        if (depth < 0)
        {
            throw new ArgumentException("Глубина не может быть отрицательной!");
        }
        return new Department(id, name, identifier, parentId, path, depth, entityLifeTime1);
    }

}
