using Deploy.Core.Repositories;

namespace Deploy.Tests;

#pragma warning disable CA2007
#pragma warning disable CS8625
#pragma warning disable CS1591

public sealed class UserRepositoryTests : IDisposable
{
    #region Test definition

    #region Initialization

    private Context _context;

    private static readonly List<User> _initData = [new(1, "Petka", 21), new(2, "Vasily Ivanovich", 45)];

    private void InitializeContext()
    {
        var contextOptions = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(nameof(UserRepositoryTests))
            .Options;

        _context = new Context(contextOptions);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _context.AddRange(_initData);

        _context.SaveChanges();
    }

    #endregion

    #region Inputs

    private static int _getUserAsyncTestInput = 1;
    private static User _addUserAsyncTestInput = new(3, "Nahida", 500);
    private static User _deleteUserAsyncTestInput = _initData[0];
    private static User _updateUserAsyncTestInput = _initData[1];

    #endregion

    #region Expected

    private static User _getUserAsyncTestExpected = _initData.First(u => u.Id == _getUserAsyncTestInput);
    private static User _addUserAsyncTestExpected = _addUserAsyncTestInput;
    private static User _deleteUserAsyncTestExpected = _deleteUserAsyncTestInput;
    private static User _updateUserAsyncTestExpected = new(_initData[1].Id, "Chapaev", _initData[1].Age);

    #endregion

    public void Dispose() => _context.Dispose();

    #endregion

    [SetUp]
    public void Setup() => InitializeContext();

    [Test]
    public void ConstructorTest()
    {
        var userRepository = new UserRepository(_context);

        Assert.That(userRepository, Is.Not.Null);

        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new UserRepository(null);
        });
    }

    [Test]
    public async Task GetUserAsyncTest()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var input = _getUserAsyncTestInput;
        var expected = _getUserAsyncTestExpected;

        // Act
        var result = await userRepository.GetUserAsync(input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task AddUserAsyncTest()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var input = _addUserAsyncTestInput;
        var expected = _addUserAsyncTestExpected;

        // Act
        (var success, var result) = await userRepository.AddUserAsync(input);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(expected));
        });
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await userRepository.AddUserAsync(null);
        });
    }

    [Test]
    public async Task DeleteUserAsyncTest()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var input = _deleteUserAsyncTestInput;
        var expected = _deleteUserAsyncTestExpected;

        // Act
        (var success, var result) = await userRepository.DeleteUserAsync(input);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(expected));
        });
        Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.DeleteUserAsync(null));
        Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await userRepository.DeleteUserAsync(new()));
    }

    [Test]
    public async Task UpdateUserAsyncTest()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var input = _updateUserAsyncTestInput;
        var expected = _updateUserAsyncTestExpected;

        // Act
        var success = await userRepository.UpdateUserAsync(input, expected);
        var result = await userRepository.GetUserAsync(expected.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(expected).UsingPropertiesComparer());
        });
        Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.UpdateUserAsync(null, new()));
        Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.UpdateUserAsync(new(), null));
        await Assert.ThatAsync(async () => await userRepository.UpdateUserAsync(new(), new()), Is.EqualTo(0));
    }
}
