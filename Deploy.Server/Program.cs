using Microsoft.AspNetCore.Server.Kestrel.Core;
using Deploy.Core;
using Microsoft.OpenApi.Models;

namespace Deploy.Server;

#pragma warning disable CS1591
#pragma warning disable CA2007

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel((context, options) =>
        {
            options.ListenAnyIP(5230, listenOptions =>
            {
                // for http1.1 and http2 support set to: Http1AndHttp2AndHttp3
                // listenOptions.Protocols = HttpProtocols.Http3;
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                listenOptions.UseHttps();
            });
        });

        builder.Services.AddAntiforgery();

        builder.Services.AddDbContext<Context>();

        // for controllers-based approach
        builder.Services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "My API - V1",
                Version = "v1",
                Description = "A sample API to demo Swashbuckle",
                Contact = new OpenApiContact
                {
                    Name = "senketsu03",
                    Email = "test@test.test"
                },
                License = new OpenApiLicense
                {
                    Name = "GPL-3.0-only",
                    Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.txt")
                }
            });

            // uses reflection
            // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{nameof(Deploy)}.{nameof(Server)}.xml");
            options.IncludeXmlComments(xmlPath);
        });

        var app = builder.Build();

        // for swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        // for redoc, path /api-docs by default
        app.UseReDoc();

        app.UseHttpsRedirection();

        await InitDbAsync().ConfigureAwait(false);

        // for controllers-based approach
        app.MapControllers();

        await app.RunAsync().ConfigureAwait(false);
    }

    public static async Task InitDbAsync()
    {
        await using var db = new Context();

        await db.Database.EnsureDeletedAsync().ConfigureAwait(false);
        await db.Database.EnsureCreatedAsync().ConfigureAwait(false);

        var vasya = new User() { Name = "Vasya", Age = 40 };
        var petya = new User() { Name = "Petya", Age = 30 };
        var katya = new User() { Name = "Katya", Age = 20 };

        await db.AddRangeAsync(vasya, petya, katya).ConfigureAwait(false);

        await db.SaveChangesAsync().ConfigureAwait(false);
    }

    public static User? GetUser(Context db, int id)
    {
        if (db == null) throw new ArgumentNullException(nameof(db));

        return db.Users.FirstOrDefault(u => u.Id == id);
    }

    public static async Task AddUserAsync(Context db, User user)
    {
        if (db == null) throw new ArgumentNullException(nameof(db));

        await db.Users.AddAsync(user).ConfigureAwait(false);

        await db.SaveChangesAsync().ConfigureAwait(false);
    }

    public static async Task UpdateUserAsync(Context db, int id, User user)
    {
        if (db == null) throw new ArgumentNullException(nameof(db));

        var userToUpdate = db.Users.FirstOrDefault(u => u.Id == id);

        if (user == null) throw new ArgumentNullException(nameof(user));

        UpdateUser(ref userToUpdate!, user);

        await db.SaveChangesAsync().ConfigureAwait(false);
    }

    private static void UpdateUser(ref User userToUpdate, User update)
    {
        userToUpdate.Name = update.Name;
        userToUpdate.Age = update.Age;
        userToUpdate.Company = update.Company;
    }
}

#pragma warning restore CS1591
#pragma warning restore CA2007
