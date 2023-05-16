namespace Deploy.Core;

/// <summary>
/// User
/// </summary>
public class User
{
    /// <summary>
    /// User id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// User age
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// User company
    /// </summary>
    public Company? Company { get; set; }
}
