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

    /// <summary>
    /// Init user
    /// </summary>
    public User() { }

    /// <summary>
    /// Init user
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="name">Name</param>
    /// <param name="age">Age</param>
    public User(int id, string name, int age) => (Id, Name, Age) = (id, name, age);

    /// <summary>
    /// Add age
    /// </summary>
    /// <param name="addNumber">Add number</param>
    public void AddAge(int addNumber)
    {
        Age += addNumber;
    }
}
