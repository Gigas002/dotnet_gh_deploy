using Deploy.Core;
using Deploy.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

namespace Deploy.Server;

#pragma warning disable CS1591

public class WebApplicationBuilderConfigurator(string[] args)
{
    private WebApplicationBuilder Builder { get; init; } = WebApplication.CreateBuilder(args);

    private void ConfigureNSwag()
    {
        Builder.Services.AddEndpointsApiExplorer();

        var openApiInfo = new NSwag.OpenApiInfo
        {
            Title = "My API - V1",
            Version = "v1",
            Description = "A sample API to demo NSwag",
            Contact = new NSwag.OpenApiContact
            {
                Name = "gigas002",
                Email = "test@test.test"
            },
            License = new NSwag.OpenApiLicense
            {
                Name = "GPL-3.0-only",
                Url = "https://www.gnu.org/licenses/gpl-3.0.txt"
            }
        };

        Builder.Services.AddOpenApiDocument(options =>
        {
            options.PostProcess = document => {
                document.Info = openApiInfo;
            };
        });
    }

    private void ConfigureSwashbuckle()
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        Builder.Services.AddEndpointsApiExplorer();

        var openApiInfo = new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "My API - V1",
            Version = "v1",
            Description = "A sample API to demo Swashbuckle",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "gigas002",
                Email = "test@test.test"
            },
            License = new Microsoft.OpenApi.Models.OpenApiLicense
            {
                Name = "GPL-3.0-only",
                Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.txt")
            }
        };

        Builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", openApiInfo);

            // uses reflection
            // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{nameof(Deploy)}.{nameof(Server)}.xml");
            options.IncludeXmlComments(xmlPath);
        });
    }

    private void ConfigureKestrel()
    {
        Builder.WebHost.ConfigureKestrel((_, options) =>
        {
            options.ListenAnyIP(5230, listenOptions =>
            {
                // for http1.1 and http2 support set to: Http1AndHttp2AndHttp3
                // listenOptions.Protocols = HttpProtocols.Http3;
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                listenOptions.UseHttps();
            });
        });
    }

    private void ConfigureAntiforgery()
    {
        Builder.Services.AddAntiforgery();
        Builder.Services.AddMvc(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });
    }

    private void ConfigureServices()
    {
        Builder.Services.AddScoped<IUserRepository, UserRepository>();
    }

    private void ConfigureDbContext()
    {
        Builder.Services.AddDbContext<Context>(options => options.UseSqlite(DbContextConfigurator.DataSource).UseSnakeCaseNamingConvention());
    }

    private void ConfigureControllers()
    {
        Builder.Services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
    }

    public static WebApplicationBuilder Configure(string[] args, bool isSwashbuckle)
    {
        var configurator = new WebApplicationBuilderConfigurator(args);
        configurator.ConfigureKestrel();
        configurator.ConfigureAntiforgery();
        configurator.ConfigureDbContext();
        configurator.ConfigureControllers();
        configurator.ConfigureServices();

        if (isSwashbuckle) configurator.ConfigureSwashbuckle();
        else configurator.ConfigureNSwag();
        
        return configurator.Builder;
    }
}
