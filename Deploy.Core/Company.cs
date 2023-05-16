namespace Deploy.Core;

/// <summary>
/// Company
/// </summary>
public class Company
{
    /// <summary>
    /// Company id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Company name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Company users
    /// </summary>
    public IEnumerable<User> Users { get; } = new List<User>();

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
