using DirectoryService.Domain.valueObject;

namespace DirectoryService.Domain;

public class Location
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public string Timezone { get; private set; }
    public EntityLifeTime EntityLifeTime { get; private set; }
    private Location(Guid id, string name, string address, string timezone, EntityLifeTime entityLifeTime1)
    {
        Id = id;
        Name = name;
        Address = address;
        Timezone = timezone;
        EntityLifeTime = entityLifeTime1;
    }
    public static Location Create(Guid id, string name, string address, string timezone, EntityLifeTime entityLifeTime1)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Пустой идентификатор!");
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException("Пустое имя!");
        }
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentNullException("Пустой адрес!");
        }
        if (string.IsNullOrWhiteSpace(timezone))
        {
            throw new ArgumentNullException("Пустая временная зона!");
        }
        return new Location(id, name, address, timezone, entityLifeTime1);
    }
}
