using Deploy.Server.Controllers;

namespace Deploy.Server.Tests;

public class UserControllerTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void EmptyConstructorTest()
    {
        var userController = new UserController(null);
    }
}
