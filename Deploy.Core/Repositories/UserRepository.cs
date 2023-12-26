namespace Deploy.Core.Repositories;

/// <inheritdoc />
public class UserRepository : IUserRepository
{
    private readonly Context _context;

    /// <summary>
    /// Create user repository
    /// </summary>
    /// <param name="context">Database context</param>
    public UserRepository(Context context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    /// <inheritdoc />
    public ValueTask<User?> GetUserAsync(int id) => _context.Users.FindAsync(id);

    /// <inheritdoc />
    public async ValueTask<(int, User?)> AddUserAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var added = await _context.Users.AddAsync(user).ConfigureAwait(false);
        var saveResult = await _context.SaveChangesAsync().ConfigureAwait(false);

        return (saveResult, added.Entity);
    }

    /// <inheritdoc />
    public async ValueTask<(int, User?)> DeleteUserAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var removed = _context.Users.Remove(user);
        var saveResult = await _context.SaveChangesAsync().ConfigureAwait(false);

        return (saveResult, removed.Entity);
    }

    /// <inheritdoc />
    public Task<int> UpdateUserAsync(User userToUpdate, User update)
    {
        UpdateUser(ref userToUpdate, update);

        return _context.SaveChangesAsync();
    }

    private static void UpdateUser(ref User userToUpdate, User update)
    {
        ArgumentNullException.ThrowIfNull(userToUpdate);
        ArgumentNullException.ThrowIfNull(update);

        userToUpdate.Name = update.Name;
        userToUpdate.Age = update.Age;
        userToUpdate.Company = update.Company;
    }
}
