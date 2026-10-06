using System.Text.RegularExpressions;
using DirectoryService.Domain.utilities;

namespace DirectoryService.Domain.valueObject;

public record ianaTimezone
{
    public NotEmptyString Value { get; }
    private ianaTimezone(NotEmptyString notEmpty)
    {
        Value = notEmpty;
    }
    private static Regex timezoneTemplateShablone = new Regex(@"(\w+)\b[//](\w+)\b", RegexOptions.Compiled);
    public static ianaTimezone Create(NotEmptyString notEmptyString)
    {
        if (timezoneTemplateShablone.IsMatch(notEmptyString.Value) != true)
        {
            throw new ArgumentException("Неверный шаблон!");
        }
        return new ianaTimezone(notEmptyString);
    }
}
