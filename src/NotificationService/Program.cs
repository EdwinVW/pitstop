using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.UseRabbitMQMessageHandler(builder.Configuration);

builder.Services.AddTransient<INotificationRepository>((svc) =>
{
    var sqlConnectionString = builder.Configuration.GetConnectionString("NotificationServiceCN");
    return new SqlServerNotificationRepository(sqlConnectionString);
});

builder.Services.AddTransient<IEmailNotifier>((svc) =>
{
    var mailConfigSection = builder.Configuration.GetSection("Email");
    string mailHost = mailConfigSection["Host"];
    int mailPort = Convert.ToInt32(mailConfigSection["Port"]);
    string mailUserName = mailConfigSection["User"];
    string mailPassword = mailConfigSection["Pwd"];
    return new SMTPEmailNotifier(mailHost, mailPort, mailUserName, mailPassword);
});

builder.Services.AddHostedService<NotificationWorker>();

var host = builder.Build();
await host.RunAsync();