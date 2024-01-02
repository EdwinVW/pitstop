using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.UseRabbitMQMessagePublisher(builder.Configuration);
builder.Services.AddHostedService<TimeWorker>();

var host = builder.Build();

await host.RunAsync();