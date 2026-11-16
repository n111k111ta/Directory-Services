using DirectoryService.Domain.utilities;
using DirectoryService.Domain.valueObject;

namespace Test;

public class DomainTests
{
    [Fact]
    public void InvalidTimeZoneFormatIsRejected()
    {
        var timeZone = NotEmptyString.Create("Ru");

        Assert.Throws<ArgumentException>(() => ianaTimezone.Create(timeZone));
    }
}
