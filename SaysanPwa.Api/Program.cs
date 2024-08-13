using SaysanPwa.Api.Configurations;

namespace SaysanPwa.Api;


public class Program : ApplicationStartup<SaysanStartup>
{
    static async Task Main(string[] args) => await RunAsync();
}


public class SaysanStartup : ApiApplicationBootstrapper
{

}