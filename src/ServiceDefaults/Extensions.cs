using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ServiceDefaults;

public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSerilog(configure =>
        {
            configure.ReadFrom.Configuration(builder.Configuration).Enrich.WithMachineName();
        });
        
        return builder;
    }
}
