namespace Deploy.Tests;

public class DumbTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void EmptyConstructorTest()
    {
        Dumb dumb = new Dumb();

        Assert.NotNull(dumb);
    }

    [Test]
    public void PropertiesTest()
    {
        // mock user
        var mockUser = new Mock<User>();

        // create dumb using ctor with user
        var dumb = new Dumb(mockUser.Object);

        // check prop getter
        Assert.NotNull(dumb.User);
    }
}
