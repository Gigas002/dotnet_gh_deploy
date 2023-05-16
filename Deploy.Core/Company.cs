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
    public IEnumerable<User> Users { get; set; } = null!;

    /// <summary>
    /// Init company
    /// </summary>
    public Company() { }

    /// <summary>
    /// Init company
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="name">Name</param>
    public Company(int id, string name) => (Id, Name) = (id, name);

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
