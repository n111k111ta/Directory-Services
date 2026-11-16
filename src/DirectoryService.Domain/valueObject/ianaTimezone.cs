using DirectoryService.Domain.utilities;

namespace DirectoryService.Domain.valueObject;

public record ianaTimezone
{
    public NotEmptyString Value { get; }
    private ianaTimezone(NotEmptyString notEmpty)
    {
        Value = notEmpty;
    }
    public static ianaTimezone Create(NotEmptyString notEmptyString)
    {
        string[] parts = notEmptyString.Value.Split('/');
        if (parts.Length < 2 || parts.Any(part => string.IsNullOrWhiteSpace(part)))
        {
            throw new ArgumentException("Неверный формат временной зоны!");
        }
        return new ianaTimezone(notEmptyString);
    }
}
