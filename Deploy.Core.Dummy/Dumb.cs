using Deploy.Core;

namespace Deploy.Core.Dummy;

/// <summary>
/// Dumb da-dumb
/// </summary>
public class Dumb
{
    
    /// <summary>
    /// Dumb user
    /// </summary>
    User User { get; set; }


    /// <summary>
    /// Create new dumb user
    /// </summary>
    public Dumb(User user) => User = user;
}
