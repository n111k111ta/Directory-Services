namespace DirectoryService.Domain;

public class Location
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Timezone { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    private Location(string name, string address, string timezone)
    {
        Name = name;
        Address = address;
        Timezone = timezone;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    public static Location Create(string name, string address, string timezone)
    {
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

        return new Location(name, address, timezone);
    }
    private Location(Guid id, string name, string address, string timezone, DateTime createdAt, DateTime updatedAt, DateTime? deletedAt = null)
    {
        Id = id;
        Name = name;
        Address = address;
        Timezone = timezone;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }


    public static Location Create(Guid id, string name, string address, string timezone, DateTime createdAt, DateTime updatedAt, DateTime? deletedAt = null)
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
        if (updatedAt < createdAt)
        {
            throw new ArgumentException("Дата обновления должна быть позже даты создания!");
        }
        if (deletedAt != null)
        {
            if (deletedAt < createdAt)
            {
                throw new ArgumentException("Дата удаления должна быть позже даты создания!");
            }
            if (deletedAt == DateTime.MinValue)
            {
                throw new ArgumentException("Дата удаления не может быть минимальной!");
            }
        }
        if (createdAt == DateTime.MinValue)
        {
            throw new ArgumentException("Дата создания не может быть минимальной!");
        }
        if (updatedAt == DateTime.MinValue)
        {
            throw new ArgumentException("Дата обновления не может быть минимальной!");
        }

        return new Location(id, name, address, timezone, createdAt, updatedAt, deletedAt);
    }
}
