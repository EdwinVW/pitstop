using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.HealthChecks;
using System.Threading.Tasks;

namespace Pitstop.APIGateway
{
    public class Program
    {
        private static IConfiguration Configuration;

        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseHealthChecks("/hc")
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);

                    // add ocelot configuration
                    string ocelotConfigPath = Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "OcelotConfig");
                    ocelotConfigPath = Path.Combine(ocelotConfigPath, hostingContext.HostingEnvironment.EnvironmentName);
                    config.AddOcelot(ocelotConfigPath);

                    config.AddEnvironmentVariables();

                    Configuration = config.Build();
                })
                .ConfigureServices(s =>
                {
                    s.AddOcelot();
                    s.AddHealthChecks(checks =>
                    {
                        checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(5));
                        checks.AddValueTaskCheck("HTTP Endpoint", () => new
                            ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
                    });
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(Configuration)
                        .CreateLogger();
                })
                .UseIISIntegration()
                .Configure(app =>
                {
                    app.UseOcelot().Wait();
                })
                .Build()
                .Run();
        }
    }
}
