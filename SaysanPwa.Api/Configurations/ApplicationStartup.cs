namespace SaysanPwa.Api.Configurations;
public abstract class ApplicationStartup<TBootstrapper> where TBootstrapper : ApplicationBootstrapper
{
    protected static async Task RunAsync()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        TBootstrapper startUp = (TBootstrapper)Activator.CreateInstance(typeof(TBootstrapper))!;
        startUp.ConfigureService(builder);
        await startUp.Configure(builder.Build(), builder.Environment);
    }
}
