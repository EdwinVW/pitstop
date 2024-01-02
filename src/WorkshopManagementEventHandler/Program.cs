using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<WorkshopManagementDBContext>("workshopmanagement");

builder.Services.UseRabbitMQMessageHandler(builder.Configuration);

builder.Services.AddHostedService<EventHandlerWorker>();

var host = builder.Build();

using var scope = host.Services.CreateScope();
using var dbContext = scope.ServiceProvider.GetService<WorkshopManagementDBContext>();

await dbContext.Database.MigrateAsync();

await host.RunAsync();