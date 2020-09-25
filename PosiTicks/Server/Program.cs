using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace PosiTicks.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // Configure serilog using inline initialization instead of the preferred way.
                // One downside: startup exceptions are not caught and logged
                // configure serilog in such a way that it ignores all other providers
                .UseSerilog(
                    (hostingContext, services, loggerConfiguration) =>
                        loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        // TODO: when adding another sink, like NewRelic, ensure to enable structured logging by passing.
                        // If the sink does not auto-convert to JSON, look at passing it new RenderedCompactJsonFormatter()
                    // , writeToProviders: true enable if deciding to use the other providers such as the built-in Console logging provider
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
