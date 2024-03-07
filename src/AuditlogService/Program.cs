using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.UseRabbitMQMessageHandler(builder.Configuration);

builder.Services.AddTransient(svc =>
{
    var auditlogConfigSection = builder.Configuration.GetSection("Auditlog");
    string logPath = auditlogConfigSection["path"];
    return new AuditlogWorkerConfig { LogPath = logPath };
});

builder.Services.AddHostedService<AuditLogWorker>();

var host = builder.Build();
await host.RunAsync();