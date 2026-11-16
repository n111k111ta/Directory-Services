using DirectoryService.Domain.utilities;
using DirectoryService.Domain.valueObject;

namespace DirectoryService.Domain;

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

public class Department
{
    private readonly List<Department> _children = [];
    private readonly List<DepartmentLocation> _locations = [];
    private readonly List<DepartmentPosition> _positions = [];
    public DepartmentId Id { get; private set; }
    public NotEmptyString Name { get; private set; }
    public NotEmptyString Identifier { get; private set; }
    public DepartmentId? ParentId { get; private set; }
    public DepartmentPath Path { get; private set; }
    public short Depth { get; private set; }
    public EntityLifeTime EntityLifeTime { get; private set; }
    public IReadOnlyCollection<DepartmentLocation> Locations => _locations.AsReadOnly();
    public IReadOnlyCollection<DepartmentPosition> Positions => _positions.AsReadOnly();
    public IReadOnlyCollection<Department> Children => _children.AsReadOnly();

    public void AddChilde(Department department)
    {
        CheckArhiveDepartment();
        department.CheckArhiveDepartment();
        if (Id == department.Id || department.ContainsChild(Id))
        {
            throw new InvalidOperationException("Нельзя присоединить подразделение к себе или своему потомку!");
        }
        if (department.ParentId != null)
        {
            throw new InvalidOperationException("Подразделение уже добавлено в родительское подразделение!");
        }
        if (_children.Any(x => x.Id == department.Id))
        {
            throw new InvalidOperationException("Подразделение уже добавлено в родительское подразделение!");
        }

        department.CheckTree();
        department.ParentId = Id;
        department.UpdatePath(Path, Depth);
        _children.Add(department);
        this.UpdateLastTimeChanged();
    }

    private bool ContainsChild(DepartmentId id)
    {
        return _children.Any(x => x.Id == id || x.ContainsChild(id));
    }

    private void CheckTree()
    {
        CheckArhiveDepartment();
        foreach (var child in _children)
        {
            child.CheckTree();
        }
    }

    private void UpdatePath(DepartmentPath parentPath, short parentDepth)
    {
        Path = DepartmentPath.Create(parentPath.Path, Identifier);
        Depth = (short)(parentDepth + 1);
        UpdateLastTimeChanged();
        foreach (var child in _children)
        {
            child.UpdatePath(Path, Depth);
        }
    }

    private void UpdateLastTimeChanged()
    {
        EntityLifeTime = EntityLifeTime.Update();
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
    public static async Task<Department> Create(
        DepartmentId id,
        NotEmptyString name,
        NotEmptyString identifier,
        DepartmentId? parentId,
        DepartmentPath path,
        short depth,
        EntityLifeTime entityLifeTime1,
        IDepartmentNameChecker checker)
    {
        if (id.Id == Guid.Empty)
        {
            throw new ArgumentException("Пустой идентификатор!");
        }

        if (depth < 0)
        {
            throw new ArgumentException("Глубина не может быть отрицательной!");
        }
        if (parentId == id)
        {
            throw new InvalidOperationException("Подразделение не может быть своим родителем!");
        }
        if (!await checker.IsUnique(name))
        {
            throw new InvalidOperationException("Department name is not unique");
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

        if (context.Identifier != null)
        {
            CheckTree();
        }

        if (context.Name != null)
        {
            if (context.Name != Name && !await checker.IsUnique(context.Name))
            {
                throw new InvalidOperationException("Department name is not unique");
            }

            Name = context.Name;
            isUpdate = true;
        }

        if (context.Identifier != null)
        {
            Identifier = context.Identifier;
            int separator = Path.Path.Value.LastIndexOf('.');
            string parentPath = separator < 0 ? "" : Path.Path.Value[..(separator + 1)];
            Path = new DepartmentPath(NotEmptyString.Create(parentPath + Identifier.Value));
            foreach (var child in _children)
            {
                child.UpdatePath(Path, Depth);
            }
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

        if (_locations.Any(x => x.LocationId == location.Id || x.Location.Address == location.Address))
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

        if (_positions.Any(x => x.PositionId == position.Id || x.Position.Name == position.Name))
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
    public Location Location { get; }
    public DepartmentId DepartmentId { get; }
    public LocationId LocationId { get; }

    public DepartmentLocation(Department department, Location location)
    {
        Location = location;
        DepartmentId = department.Id;
        LocationId = location.Id;
    }
}
public sealed class DepartmentPosition
{
    public Position Position { get; }
    public DepartmentId DepartmentId { get; }
    public PositionId PositionId { get; }
    public DepartmentPosition(Department department, Position position)
    {
        Position = position;
        DepartmentId = department.Id;
        PositionId = position.Id;
    }
}
