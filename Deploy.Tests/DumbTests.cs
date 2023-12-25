using Deploy.Core.Dummy;

namespace Deploy.Tests;

#pragma warning disable CS1591

public class DumbTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void EmptyConstructorTest()
    {
        var dumb = new Dumb();

        Assert.That(dumb, Is.Not.Null);
    }

    [Test]
    public void PropertiesTest()
    {
        // mock user
        var mockUser = new Mock<User>();

        // create dumb using ctor with user
        var dumb = new Dumb(mockUser.Object);

        // check prop getter
        Assert.That(dumb.User, Is.Not.Null);
    }
}
