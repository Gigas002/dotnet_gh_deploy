using Microsoft.EntityFrameworkCore;

namespace Deploy.Core;

/// <summary>
/// Database context
/// </summary>
public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    /// <summary>
    /// Users table
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Companies table
    /// </summary>
    public DbSet<Company> Companies { get; set; } = null!;
}
