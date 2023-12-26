using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Deploy.Core.Repositories;

/// <summary>
/// Repository pattern to communicate to database
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Get user from database by id
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns>User</returns>
    public ValueTask<User?> GetUserAsync(int id);

    /// <summary>
    /// Add user to database; don't forget to call SaveChangesAsync afterwards
    /// </summary>
    /// <param name="user">User to add</param>
    /// <returns>Save changes result and user</returns>
    public ValueTask<(int, User?)> AddUserAsync(User user);

    /// <summary>
    /// Delete user from database
    /// </summary>
    /// <param name="user">User to delete</param>
    /// <returns>Save changes result and user</returns>
    public ValueTask<(int, User?)> DeleteUserAsync(User user);

    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="userToUpdate">User to update</param>
    /// <param name="update">Changes to user to update</param>
    /// <returns>Save changes result</returns>
    public Task<int> UpdateUserAsync(User userToUpdate, User update);
}
