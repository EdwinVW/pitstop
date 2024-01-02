using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
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

        builder.Services.AddResiliencePipeline("Retry", builder =>
        {
            builder.AddRetry(new Polly.Retry.RetryStrategyOptions
            {
                Delay = TimeSpan.FromSeconds(1),
                MaxRetryAttempts = 3,
            });
        });

        return builder;
    }
}
