using DirectoryService.Domain.utilities;
using DirectoryService.Domain.valueObject;

namespace DirectoryService.Domain;


public class Position
{
    public NotEmptyString Name { get; private set; }
    public NotEmptyString Description { get; private set; }
    public Guid Id { get; private set; }
    public EntityLifeTime EntityLifeTime { get; private set; }

    private Position(
        NotEmptyString name,
        NotEmptyString description,
        Guid id,
        EntityLifeTime entityLifeTime)
    {
        Name = name;
        Description = description;
        Id = id;
        EntityLifeTime = entityLifeTime;
    }

    public static Position Create(
        NotEmptyString name,
        NotEmptyString description,
        Guid id,
        EntityLifeTime entityLifeTime1)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Пустой идентификатор!");
        }
        return new Position(name, description, id, entityLifeTime1);
    }

    public async Task ChangeName(NotEmptyString newName, IPositonNameChecker checker)
    {
        if (!await checker.IsUnique(newName))
        {
            throw new InvalidOperationException("Position name is not unique");
        }

        Name = newName;
    }
    private void CheckArhiveLocation()
    {
        if (EntityLifeTime.IsDeleted())
        {
            throw new Exception("Архивный объект не может быть изменен!");
        }
    }
    public void Update(UpdateContext context)
    {
        bool isUpdate = true;
        CheckArhiveLocation();
        if (context.Description != null)
        {
            Description = context.Description;
            isUpdate = true;
        }
        if (context.Name != null)
        {
            Name = context.Name;
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
}
public record UpdateContext(NotEmptyString? Name, NotEmptyString? Description);
public interface IPositonNameChecker
{
    Task<bool> IsUnique(NotEmptyString uniqueName);
}
