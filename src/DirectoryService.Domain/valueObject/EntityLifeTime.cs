namespace DirectoryService.Domain.valueObject;

public readonly record struct EntityLifeTime
{
    public DateTime CreatedAt { get; }
    public DateTime? DeletedAt { get; }
    public DateTime UpdatedAt { get; }

    public EntityLifeTime()
    {
        var dateTime = DateTime.UtcNow;
        CreatedAt = dateTime;
        DeletedAt = null;
        UpdatedAt = dateTime;
    }
    private EntityLifeTime(DateTime createdAt, DateTime? deletedAt, DateTime updatedAt)
    {
        CreatedAt = createdAt;
        DeletedAt = deletedAt;
        UpdatedAt = updatedAt;
    }
    public static EntityLifeTime Create(DateTime createdAt, DateTime? deletedAt, DateTime updatedAt)
    {
    if (updatedAt < createdAt)
        {
            throw new ArgumentException("Дата обновления должна быть позже даты создания!");
        }
        if (deletedAt != DateTime.MinValue)
        {
            if (deletedAt < createdAt)
            {
                throw new ArgumentException("Дата удаления должна быть позже даты создания!");
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
        return new EntityLifeTime(createdAt, deletedAt, updatedAt);
    }
    public static EntityLifeTime CreatedNew()
    {
        return new EntityLifeTime();
    }
    public bool IsDeleted()
    {
        return DeletedAt != null;
    }
}
