namespace SaysanPwa.Api.Configurations;

public abstract class ApplicationBootstrapper
{
    public IConfiguration Configuration { get; set; }

    public ApplicationBootstrapper()
    {
        Configuration = GetConfiguration();
    }

    public virtual IConfiguration GetConfiguration() =>
        new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .AddEnvironmentVariables()
        .Build();


    public virtual void ConfigureService(WebApplicationBuilder builder)
    {
        builder.Configuration.AddConfiguration(Configuration);
    }


    public virtual async Task Configure(WebApplication app, IWebHostEnvironment env)
    {
        await app.RunAsync();
    }
}
