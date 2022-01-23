IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.UseRabbitMQMessageHandler(hostContext.Configuration);

        services.AddTransient<INotificationRepository>((svc) =>
        {
            var sqlConnectionString = hostContext.Configuration.GetConnectionString("NotificationServiceCN");
            return new SqlServerNotificationRepository(sqlConnectionString);
        });

        services.AddTransient<IEmailNotifier>((svc) =>
        {
            var mailConfigSection = hostContext.Configuration.GetSection("Email");
            string mailHost = mailConfigSection["Host"];
            int mailPort = Convert.ToInt32(mailConfigSection["Port"]);
            string mailUserName = mailConfigSection["User"];
            string mailPassword = mailConfigSection["Pwd"];
            return new SMTPEmailNotifier(mailHost, mailPort, mailUserName, mailPassword);
        });

        services.AddHostedService<NotificationWorker>();
    })
    .UseSerilog((hostContext, loggerConfiguration) =>
    {
        loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();