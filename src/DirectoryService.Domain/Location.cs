using DirectoryService.Domain.utilities;
using DirectoryService.Domain.valueObject;

namespace DirectoryService.Domain;

public class Location
{
    public Guid Id { get; private set; }
    public NotEmptyString Name { get; private set; }
    public NotEmptyString Address { get; private set; }
    public ianaTimezone Timezone { get; private set; }
    public EntityLifeTime EntityLifeTime { get; private set; }
    private Location(Guid id, NotEmptyString name, NotEmptyString address, ianaTimezone timezone, EntityLifeTime entityLifeTime1)
    {
        Id = id;
        Name = name;
        Address = address;
        Timezone = timezone;
        EntityLifeTime = entityLifeTime1;
    }
    public static Location Create(Guid id, NotEmptyString name, NotEmptyString address, ianaTimezone timezone, EntityLifeTime entityLifeTime1)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Пустой идентификатор!");
        }
        return new Location(id, name, address, timezone, entityLifeTime1);
    }

}
