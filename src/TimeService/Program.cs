IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.UseRabbitMQMessagePublisher(hostContext.Configuration);

        services.AddHostedService<TimeWorker>();
    })
    .UseSerilog((hostContext, loggerConfiguration) =>
    {
        loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();