namespace Deploy.Server;

#pragma warning disable CS1591

public class WebApplicationConfigurator(WebApplicationBuilder builder)
{
    private WebApplication Application { get; init; } = builder.Build();

    private void ConfigureNSwag()
    {
        Application.UseOpenApi();
        Application.UseSwaggerUi();

        Application.UseReDoc(options =>
        {
            options.Path = "/api-docs";
        });
    }

    private void ConfigureSwashbuckle()
    {
        Application.UseSwagger();
        Application.UseSwaggerUI();

        // for redoc, path /api-docs by default
        // Application.UseReDoc(options: null);
    }

    private void ConfigureHsts()
    {
        Application.UseHttpsRedirection();
        Application.UseHsts();
    }

    private void ConfigureControllers()
    {
        Application.MapControllers();
    }

    public static WebApplication Configure(WebApplicationBuilder builder, bool isSwashbuckle)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var configurator = new WebApplicationConfigurator(builder);
        configurator.ConfigureHsts();
        configurator.ConfigureControllers();

        if (isSwashbuckle) configurator.ConfigureSwashbuckle();
        else configurator.ConfigureNSwag();
    
        return configurator.Application;
    }
}
