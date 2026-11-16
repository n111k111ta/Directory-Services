using DirectoryService.Domain.utilities;

namespace DirectoryService.Domain;

public class PositionNameChecker : IPositonNameChecker
{
    private readonly IReadOnlyCollection<Position> _positions;

    public PositionNameChecker(IReadOnlyCollection<Position> positions)
    {
        _positions = positions;
    }

    public Task<bool> IsUnique(NotEmptyString uniqueName)
    {
        return Task.FromResult(!_positions.Any(x => x.Name == uniqueName));
    }
}

public class DepartmentNameChecker : IDepartmentNameChecker
{
    private readonly IReadOnlyCollection<Department> _departments;

    public DepartmentNameChecker(IReadOnlyCollection<Department> departments)
    {
        _departments = departments;
    }

    public Task<bool> IsUnique(NotEmptyString uniqueName)
    {
        return Task.FromResult(!_departments.Any(x => x.Name == uniqueName));
    }
}
