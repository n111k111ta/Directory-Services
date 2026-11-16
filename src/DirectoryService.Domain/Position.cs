using DirectoryService.Domain.utilities;
using DirectoryService.Domain.valueObject;

namespace DirectoryService.Domain;


public class Position
{
    public NotEmptyString Name { get; private set; }
    public NotEmptyString Description { get; private set; }
    public PositionId Id { get; private set; }
    public EntityLifeTime EntityLifeTime { get; private set; }

    private Position(
        NotEmptyString name,
        NotEmptyString description,
        PositionId id,
        EntityLifeTime entityLifeTime)
    {
        Name = name;
        Description = description;
        Id = id;
        EntityLifeTime = entityLifeTime;
    }

    public static async Task<Position> Create(
        NotEmptyString name,
        NotEmptyString description,
        PositionId id,
        EntityLifeTime entityLifeTime1,
        IPositonNameChecker checker)
    {
        if (id.Id == Guid.Empty)
        {
            throw new ArgumentException("Пустой идентификатор!");
        }
        if (!await checker.IsUnique(name))
        {
            throw new InvalidOperationException("Position name is not unique");
        }
        return new Position(name, description, id, entityLifeTime1);
    }

    public async Task ChangeName(NotEmptyString newName, IPositonNameChecker checker)
    {
        CheckArhiveLocation();

        if (newName != Name && !await checker.IsUnique(newName))
        {
            throw new InvalidOperationException("Position name is not unique");
        }

        Name = newName;
        EntityLifeTime = EntityLifeTime.Update();
    }
    private void CheckArhiveLocation()
    {
        if (EntityLifeTime.IsDeleted())
        {
            throw new Exception("Архивный объект не может быть изменен!");
        }
    }
    public async Task Update(UpdateContext context, IPositonNameChecker checker)
    {
        bool isUpdate = false;
        CheckArhiveLocation();
        if (context.Name != null)
        {
            await ChangeName(context.Name, checker);
            isUpdate = true;
        }
        if (context.Description != null)
        {
            Description = context.Description;
            isUpdate = true;
        }
        if (isUpdate)
        {
            EntityLifeTime = EntityLifeTime.Update();
        }
        else
        {
            throw new Exception("Необходимо обновить хотя бы одно поле!");
        }
    }

    internal void UpdateLastTimeChange()
    {
        EntityLifeTime = EntityLifeTime.Update();
    }

    internal void CheckArchievePosition()
    {
        if (EntityLifeTime.IsDeleted())
        {
            throw new Exception("Архивный объект не может быть изменен!");
        }
    }
}
public record UpdateContext(NotEmptyString? Name, NotEmptyString? Description);
public interface IPositonNameChecker
{
    Task<bool> IsUnique(NotEmptyString uniqueName);
}

public readonly record struct PositionId
{
    public Guid Id { get; }

    private PositionId(Guid id)
    {
        Id = id;
    }

    public PositionId()
    {
        Id = Guid.NewGuid();
    }

    public static PositionId Create(Guid id)
    {
        return new PositionId(id);
    }
}
