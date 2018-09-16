using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Serilog;

namespace Pitstop.Application.VehicleManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseSerilog()
                .UseHealthChecks("/hc")
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}
