IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.UseRabbitMQMessageHandler(hostContext.Configuration);

        services.AddTransient<AuditlogManagerConfig>((svc) =>
        {
            var auditlogConfigSection = hostContext.Configuration.GetSection("Auditlog");
            string logPath = auditlogConfigSection["path"];
            return new AuditlogManagerConfig { LogPath = logPath };
        });

        services.AddHostedService<AuditLogWorker>();
    })
    .UseSerilog((hostContext, loggerConfiguration) =>
    {
        loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();