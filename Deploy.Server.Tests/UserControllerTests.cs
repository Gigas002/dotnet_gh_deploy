using Deploy.Core.Repositories;
using Deploy.Server.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemTextJsonPatch;
using SystemTextJsonPatch.Operations;

namespace Deploy.Server.Tests;

#pragma warning disable CS1591
#pragma warning disable CA2007

public class UserControllerTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public async Task GetUserAsyncTest()
    {
        var testCases = new List<TestCase> { new()
        {
            Inputs = new Tuple<int, User?>(1, new User(1, "Dina Zaur", 55)),
            Expected = StatusCodes.Status200OK
        }, new()
        {
            Inputs = new Tuple<int, User?>(1, null),
            Expected = StatusCodes.Status400BadRequest
        }};

        foreach (var testCase in testCases)
        {
            // Arrange
            var repositoryMock = new Mock<IUserRepository>();
            var idMock = (testCase.Inputs as Tuple<int, User>)!.Item1;
            var userMock = (testCase.Inputs as Tuple<int, User>)!.Item2;
            var expected = (int)testCase.Expected!;
            repositoryMock.Setup(r => r.GetUserAsync(idMock))
                          .ReturnsAsync(userMock);
            var controller = new UserController(repositoryMock.Object);

            // Act
            var result = await controller.GetUserAsync(idMock);
            var statusCode = result!.GetStatusCode();
            // var user = result.GetObjectResultContent();

            // Assert
            Assert.That(statusCode, Is.EqualTo(expected));
            repositoryMock.Verify(r => r.GetUserAsync(idMock), Times.Once());
        }
    }

    [Test]
    public async Task PostUserAsyncTest()
    {
        var testCases = new List<TestCase> { new()
        {
            Inputs = new User(1, "Dina Zaur", 55),
            Expected = StatusCodes.Status201Created
        }, new()
        {
            Inputs = null,
            Expected = StatusCodes.Status400BadRequest
        }};

        foreach (var testCase in testCases)
        {
            // Arrange
            var repositoryMock = new Mock<IUserRepository>();
            var userMock = testCase.Inputs as User;
            var expected = (int)testCase.Expected!;
            repositoryMock.Setup(r => r.AddUserAsync(userMock!))
                          .ReturnsAsync((1, userMock));
            var controller = new UserController(repositoryMock.Object);

            // Act
            var result = await controller.PostUserAsync(userMock!);
            var statusCode = result!.GetStatusCode();

            // Assert
            Assert.That(statusCode, Is.EqualTo(expected));
        }
    }

    [Test]
    public async Task PatchUserAsyncTest()
    {
        var patch = new JsonPatchDocument<User>();
        patch.Replace((v) => v.Name, "Gryazevick");
        patch.Replace((v) => v.Age, 36);

        var testCases = new List<TestCase> { new()
        {
            Inputs = new Tuple<int, JsonPatchDocument<User>?, User>(1, patch, new User(1, "Dina Zaur", 55)),
            Expected = StatusCodes.Status201Created
        }, new()
        {
            Inputs = new Tuple<int, JsonPatchDocument<User>?, User>(1, null, new User(1, "Dina Zaur", 55)),
            Expected = StatusCodes.Status400BadRequest
        }};

        foreach (var testCase in testCases)
        {
            // Arrange
            var repositoryMock = new Mock<IUserRepository>();
            var idMock = (testCase.Inputs as Tuple<int, JsonPatchDocument<User>?, User>)!.Item1;
            var patchMock = (testCase.Inputs as Tuple<int, JsonPatchDocument<User>?, User>)!.Item2;
            var userMock = (testCase.Inputs as Tuple<int, JsonPatchDocument<User>?, User>)!.Item3;
            var expected = (int)testCase.Expected!;
            repositoryMock.Setup(r => r.GetUserAsync(idMock))
                          .ReturnsAsync(userMock);
            repositoryMock.Setup(r => r.UpdateUserAsync(userMock, userMock))
                                       .ReturnsAsync(1);
            var controller = new UserController(repositoryMock.Object);

            // Act
            var result = await controller.PatchUserAsync(idMock, patchMock!);
            var statusCode = result!.GetStatusCode();

            // Assert
            Assert.That(statusCode, Is.EqualTo(expected));
        }
    }

    [Test]
    public async Task PutUserAsyncTest()
    {
        var testCases = new List<TestCase> { new()
        {
            Inputs = new Tuple<int, User?>(1, new User(1, "Dina Zaur", 55)),
            Expected = StatusCodes.Status200OK
        }, new()
        {
            Inputs = new Tuple<int, User?>(1, null),
            Expected = StatusCodes.Status400BadRequest
        }};

        foreach (var testCase in testCases)
        {
            // Arrange
            var repositoryMock = new Mock<IUserRepository>();
            var idMock = (testCase.Inputs as Tuple<int, User?>)!.Item1;
            var userMock = (testCase.Inputs as Tuple<int, User?>)!.Item2;
            var expected = (int)testCase.Expected!;
            repositoryMock.Setup(r => r.GetUserAsync(idMock))
                          .ReturnsAsync(userMock);
            repositoryMock.Setup(r => r.UpdateUserAsync(userMock!, userMock!))
                                       .ReturnsAsync(1);
            var controller = new UserController(repositoryMock.Object);

            // Act
            var result = await controller.PutUserAsync(idMock, userMock!);
            var statusCode = result!.GetStatusCode();

            // Assert
            Assert.That(statusCode, Is.EqualTo(expected));
        }
    }

    [Test]
    public void TestOptionsMethod()
    {
        // Arrange
        var mockHttpContext = new Mock<HttpContext>();
        var repositoryMock = new Mock<IUserRepository>();
        mockHttpContext.Setup(c => c.Response.Headers).Returns(new HeaderDictionary
        {
            { "X-Custom-Header", "some value" }
        });
        var controller = new UserController(repositoryMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            }
        };

        // Act
        var result = controller.Options();

        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());
    }

    [Test]
    public async Task DeleteUserAsyncTest()
    {
        // Arrange
        var repositoryMock = new Mock<IUserRepository>();
        var controller = new UserController(repositoryMock.Object);
        var idMock = 1;
        var userMock = new Mock<User>(idMock, "Dina Zaur", 55);
        repositoryMock.Setup(r => r.GetUserAsync(idMock))
                      .ReturnsAsync(userMock.Object);
        var expected = StatusCodes.Status200OK;

        // Act
        var result = await controller.DeleteUserAsync(idMock);
        var statusCode = result!.GetStatusCode();

        // Assert
        Assert.That(statusCode, Is.EqualTo(expected));
    }

    [Test]
    public async Task PatchUserExpAsyncTest()
    {
        var operations = new List<Operation<User>>
        {
            new(OperationType.Replace.ToString(), "/name", null, "Nahida"),
            new("replace", "/age", null, 500)
        };

        var testCases = new List<TestCase> { new()
        {
            Inputs = new Tuple<int, IEnumerable<Operation<User>>?, User>(1, operations, new User(1, "Dina Zaur", 55)),
            Expected = StatusCodes.Status201Created
        }, new()
        {
            Inputs = new Tuple<int, IEnumerable<Operation<User>>?, User>(1, null, new User(1, "Dina Zaur", 55)),
            Expected = StatusCodes.Status400BadRequest
        }};

        foreach (var testCase in testCases)
        {
            // Arrange
            var repositoryMock = new Mock<IUserRepository>();
            var idMock = (testCase.Inputs as Tuple<int, IEnumerable<Operation<User>>?, User>)!.Item1;
            var operationsMock = (testCase.Inputs as Tuple<int, IEnumerable<Operation<User>>?, User>)!.Item2;
            var userMock = (testCase.Inputs as Tuple<int, IEnumerable<Operation<User>>?, User>)!.Item3;
            var expected = (int)testCase.Expected!;
            repositoryMock.Setup(r => r.GetUserAsync(idMock))
                          .ReturnsAsync(userMock);
            repositoryMock.Setup(r => r.UpdateUserAsync(userMock, userMock))
                                       .ReturnsAsync(1);
            var controller = new UserController(repositoryMock.Object);

            // Act
            var result = await controller.PatchUserExpAsync(idMock, operationsMock!);
            var statusCode = result!.GetStatusCode();

            // Assert
            Assert.That(statusCode, Is.EqualTo(expected));
        }
    }

    [Test]
    public void IgnoredMethodTest()
    {
        var repositoryMock = new Mock<IUserRepository>();
        var controller = new UserController(repositoryMock.Object);
        controller.IgnoredMethod();
    }
}
