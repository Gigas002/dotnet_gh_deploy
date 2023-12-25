using Microsoft.EntityFrameworkCore;

namespace Deploy.Core;

/// <summary>
/// Database context
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Users table
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Companies table
    /// </summary>
    public DbSet<Company> Companies { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        if (optionsBuilder.IsConfigured) return;

        optionsBuilder.UseSqlite("Data Source=../deploy.db")
                      .UseSnakeCaseNamingConvention();
    }

    /// <inheritdoc/>
    public Context() : base()
    { }

    /// <inheritdoc/>
    public Context(DbContextOptions<Context> options) : base(options)
    { }
}
