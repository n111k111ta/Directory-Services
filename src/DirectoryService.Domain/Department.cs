using DirectoryService.Domain.utilities;
using DirectoryService.Domain.valueObject;

namespace DirectoryService.Domain;

public class Department
{
    public Guid Id { get; private set; }
    public NotEmptyString Name { get; private set; }
    public NotEmptyString Identifier { get; private set; }
    public Guid? ParentId { get; private set; }
    public NotEmptyString Path { get; private set; }
    public short Depth { get; private set; }
    public EntityLifeTime EntityLifeTime { get; private set; }
    private Department(Guid id, NotEmptyString name, NotEmptyString identifier, Guid? parentId, NotEmptyString path, short depth, EntityLifeTime entityLifeTime1)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        EntityLifeTime = entityLifeTime1;
    }
    public static Department Create(Guid id, NotEmptyString name, NotEmptyString identifier, Guid parentId, NotEmptyString path, short depth, EntityLifeTime entityLifeTime1)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Пустой идентификатор!");
        }

        if (depth < 0)
        {
            throw new ArgumentException("Глубина не может быть отрицательной!");
        }
        return new Department(id, name, identifier, parentId, path, depth, entityLifeTime1);
    }

}
