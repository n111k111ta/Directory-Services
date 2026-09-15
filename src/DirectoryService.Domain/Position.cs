using DirectoryService.Domain.valueObject;

namespace DirectoryService.Domain;


public class Position
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid Id { get; private set; }
    public EntityLifeTime EntityLifeTime { get; private set; }

    private Position(string name, string description, Guid id, EntityLifeTime entityLifeTime1)
    {
        Name = name;
        Description = description;
        Id = id;
        EntityLifeTime = entityLifeTime1;
    }

    public static Position Create(string name, string description, Guid id, EntityLifeTime entityLifeTime1)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Пустой идентификатор!");
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException("Пустое имя!");
        }
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentNullException("Пустое описание!");
        }
        return new Position(name, description, id, entityLifeTime1);
    }

}