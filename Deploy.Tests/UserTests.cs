namespace Deploy.Tests;

#pragma warning disable CS1591

public class UserTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void EmptyConstructorTest()
    {
        var user = new User();

        Assert.That(user, Is.Not.Null);
    }

    [Test]
    public void PropertiesTest()
    {
        // ctor and setters
        var user = new User(1, "Tom", 30);

        // id
        var id = user.Id;
        Assert.That(id, Is.EqualTo(user.Id));

        // name
        var name = user.Name;
        Assert.That(name, Is.EqualTo(user.Name));

        // age
        var age = user.Age;
        Assert.That(age, Is.EqualTo(user.Age));

        // company
        var mockCompany = new Mock<Company>();
        user.Company = mockCompany.Object;
        Assert.That(user.Company, Is.Not.Null);
    }

    [TestCase(1)]
    [TestCase(2)]
    public void AddAgeTest(int addNumber)
    {
        var user = new User(1, "Tom", 30);

        var actual = user.Age + addNumber;

        user.AddAge(addNumber);

        Assert.That(actual, Is.EqualTo(user.Age));
    }

    [Test]
    public void CloneTest()
    {
        var user = new User(1, "Tom", 30);

        var clone = user.Clone();

        Assert.Multiple(() =>
        {
            Assert.That(user.Name, Is.EqualTo(clone.Name));
            Assert.That(user.Age, Is.EqualTo(clone.Age));
            Assert.That(user.Company, Is.EqualTo(clone.Company));
        });
    }
}