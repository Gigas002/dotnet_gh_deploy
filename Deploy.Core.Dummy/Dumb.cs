namespace Deploy.Core.Dummy;

/// <summary>
/// Dumb da-dumb
/// </summary>
public class Dumb
{
    /// <summary>
    /// Dumb user
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Create dumb
    /// </summary>
    public Dumb() => User = new User();

    /// <summary>
    /// Create new dumb user
    /// </summary>
    public Dumb(User user) => User = user;
}
