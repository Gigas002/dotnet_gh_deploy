using Microsoft.EntityFrameworkCore;

namespace Deploy.Core;

/// <summary>
/// Database context
/// </summary>
public class Context : DbContext
{
    private readonly StreamWriter _logStream = new StreamWriter("log.txt", true);

    /// <summary>
    /// Users table
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Companies table
    /// </summary>
    public DbSet<Company> Companies { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=../deploy.db")
                      .UseSnakeCaseNamingConvention()
                      .LogTo(_logStream.WriteLine);
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();
        _logStream.Dispose();
    }

    /// <inheritdoc/>
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _logStream.DisposeAsync();
    }
}
