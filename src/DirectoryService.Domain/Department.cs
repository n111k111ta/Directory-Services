using DirectoryService.Domain.utilities;
using DirectoryService.Domain.valueObject;

namespace DirectoryService.Domain;

public interface ILifeTimeAble
{
    EntityLifeTime EntityLifeTime { get; set; }
}

public static class LifeTimeAbleExtensions
{
    extension(ILifeTimeAble lifeTimeAble)
    {
        public void UpdateLastTimeChanged()
        {
            EntityLifeTime lifetime = lifeTimeAble.EntityLifeTime.Update();
        }
    }
}

public record DepartmentPath
{
    public NotEmptyString Path { get; }
    public DepartmentPath(NotEmptyString path)
    {
        Path = path;
    }
    public static DepartmentPath Create(NotEmptyString path1, NotEmptyString path2)
    {
        var Path = path1.Value + "." + path2.Value;
        return new DepartmentPath(NotEmptyString.Create(Path));
    }

}

public class Department : ILifeTimeAble
{
    private readonly List<DepartmentLocation> _locations = [];
    private readonly List<DepartmentPosition> _positions = [];
    public DepartmentId Id { get; private set; }
    public NotEmptyString Name { get; private set; }
    public NotEmptyString Identifier { get; private set; }
    public DepartmentId? ParentId { get; private set; }
    public DepartmentPath Path { get; private set; }
    public short Depth { get; private set; }
    public EntityLifeTime EntityLifeTime { get; set; }
    public IReadOnlyCollection<DepartmentLocation> Locations => _locations;
    public IReadOnlyCollection<DepartmentPosition> Positions => _positions;

    public void AddChilde(Department department)
    {
        CheckArhiveDepartment();
        department.CheckArhiveDepartment();
        if (department.ParentId != null)
        {
            throw new InvalidOperationException("Подразделение уже добавлено в родительское подразделение!");
        }
        if (this.Path.Path.Value.Contains(department.Identifier.Value))
        {
            throw new InvalidOperationException("Подразделение уже добавлено в родительское подразделение!");
        }

        ParentId = department.Id;
        Path = DepartmentPath.Create(this.Path.Path, department.Identifier);
        Depth++;
        department.UpdateLastTimeChanged();
        this.UpdateLastTimeChanged();
    }
    private Department(
        DepartmentId id,
        NotEmptyString name,
        NotEmptyString identifier,
        DepartmentId? parentId,
        DepartmentPath path,
        short depth,
        EntityLifeTime entityLifeTime1)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        EntityLifeTime = entityLifeTime1;
    }
    public static Department Create(
        DepartmentId id,
        NotEmptyString name,
        NotEmptyString identifier,
        DepartmentId parentId,
        DepartmentPath path,
        short depth,
        EntityLifeTime entityLifeTime1)
    {
        if (id.Id == Guid.Empty)
        {
            throw new ArgumentException("Пустой идентификатор!");
        }

        if (depth < 0)
        {
            throw new ArgumentException("Глубина не может быть отрицательной!");
        }
        return new Department(id, name, identifier, parentId, path, depth, entityLifeTime1);
    }

    private void CheckArhiveDepartment()
    {
        if (EntityLifeTime.IsDeleted())
        {
            throw new Exception("Архивный объект не может быть изменен!");
        }
    }

    public async Task Update(UpdateDepartmentContext context, IDepartmentNameChecker checker)
    {
        bool isUpdate = false;
        CheckArhiveDepartment();

        if (context.Name != null)
        {
            if (!await checker.IsUnique(context.Name))
            {
                throw new InvalidOperationException("Department name is not unique");
            }

            Name = context.Name;
            isUpdate = true;
        }

        if (context.Identifier != null)
        {
            Identifier = context.Identifier;
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

    public void AddLocation(Location location)
    {
        CheckArhiveDepartment();
        location.CheckArhiveLocation();

        if (_locations.Any(x => x.LocationId == location.Id))
        {
            throw new InvalidOperationException("Локация уже добавлена в подразделение!");
        }

        DepartmentLocation _location = new(this, location);
        _locations.Add(_location);

        this.UpdateLastTimeChanged();
        location.UpdateLastTimeChange();
    }


    public void AddPosition(Position position)
    {
        CheckArhiveDepartment();
        position.CheckArchievePosition();

        if (_positions.Any(x => x.PositionId == position.Id))
        {
            throw new InvalidOperationException("Должность уже добавлена в подразделение!");
        }

        DepartmentPosition _position = new(this, position);
        _positions.Add(_position);

        this.UpdateLastTimeChanged();
        position.UpdateLastTimeChange();
    }
}

public record UpdateDepartmentContext(NotEmptyString? Name, NotEmptyString? Identifier);

public interface IDepartmentNameChecker
{
    Task<bool> IsUnique(NotEmptyString uniqueName);
}

public readonly record struct DepartmentId
{
    public Guid Id { get; }

    private DepartmentId(Guid id)
    {
        Id = id;
    }

    public DepartmentId()
    {
        Id = Guid.NewGuid();
    }

    public static DepartmentId Create(Guid id)
    {
        return new DepartmentId(id);
    }
}

public sealed class DepartmentLocation
{
    public DepartmentId DepartmentId { get; }
    public LocationId LocationId { get; }

    public DepartmentLocation(Department department, Location location)
    {
        DepartmentId = department.Id;
        LocationId = location.Id;
    }
}
public sealed class DepartmentPosition
{
    public DepartmentId DepartmentId { get; }
    public PositionId PositionId { get; }
    public DepartmentPosition(Department department, Position position)
    {
        DepartmentId = department.Id;
        PositionId = position.Id;
    }
}
