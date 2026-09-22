namespace DirectoryService.Domain.utilities;

public record NotEmptyString
{
    public string Value { get; }
    private NotEmptyString(string notEmptyString)
    {
        Value = notEmptyString;
    }
    public static NotEmptyString Create(string notEmptyString, int size)
    {
        notEmptyString = notEmptyString.Trim();
        if (string.IsNullOrWhiteSpace(notEmptyString))
        {
            throw new ArgumentException("Строка не может быть пустой!");
        }

        if (notEmptyString.Length > size)
        {
            throw new ArgumentException("Строка не может быть больше " + size + " символов!");
        }
        return new NotEmptyString(notEmptyString);
    }
}
