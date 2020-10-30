using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace PosiTicks.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // enable if using serilog from Blazor/WASM does not work as expected
            // Serilog.Debugging.SelfLog.Enable(m => Console.Error.WriteLine(m));

            // The LoggingLevelSwitch controls the minimum level of the serilog pipeline.
            // The way we use it allows the server to set the client's log level, avoiding wasted bandwidth.
            var levelSwitch = new LoggingLevelSwitch();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .Enrich.WithProperty("ClientInstanceId", Guid.NewGuid().ToString("n")) // make it easy to identify logs from a single running instance of the Blazor app (single browser tab)
                .WriteTo.BrowserHttp(controlLevelSwitch: levelSwitch)   // does not require a URL to be specified, it defaults to /ingest on the origin server
                .WriteTo.BrowserConsole()
                .CreateLogger();

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
