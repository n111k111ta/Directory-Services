using DirectoryService.Domain.utilities;
using DirectoryService.Domain.valueObject;

namespace DirectoryService.Domain;


public class Position
{
    public NotEmptyString Name { get; private set; }
    public NotEmptyString Description { get; private set; }
    public Guid Id { get; private set; }
    public EntityLifeTime EntityLifeTime { get; private set; }

    private Position(NotEmptyString name, NotEmptyString description, Guid id, EntityLifeTime entityLifeTime1)
    {
        Name = name;
        Description = description;
        Id = id;
        EntityLifeTime = entityLifeTime1;
    }

    public static Position Create(NotEmptyString name, NotEmptyString description, Guid id, EntityLifeTime entityLifeTime1)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Пустой идентификатор!");
        }
        return new Position(name, description, id, entityLifeTime1);
    }
}