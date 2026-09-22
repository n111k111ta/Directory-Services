using DirectoryService.Domain;

namespace Test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.Throws<ArgumentNullException>(() => Location.Create("test", "       test         ", "test"));
    }
}
