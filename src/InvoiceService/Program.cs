using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerClient("Invoicing");

builder.Services.UseRabbitMQMessageHandler(builder.Configuration);

builder.Services.AddTransient<IInvoiceRepository>((svc) =>
{
    var sqlConnectionString = builder.Configuration.GetConnectionString("Invoicing");
    return new SqlServerInvoiceRepository(sqlConnectionString);
});

builder.Services.AddTransient<IEmailCommunicator>((svc) =>
{
    var mailConfigSection = builder.Configuration.GetSection("Email");
    string mailHost = mailConfigSection["Host"];
    int mailPort = Convert.ToInt32(mailConfigSection["Port"]);
    string mailUserName = mailConfigSection["User"];
    string mailPassword = mailConfigSection["Pwd"];
    return new SMTPEmailCommunicator(mailHost, mailPort, mailUserName, mailPassword);
});

builder.Services.AddHostedService<InvoiceWorker>();

var host = builder.Build();

await host.RunAsync();
