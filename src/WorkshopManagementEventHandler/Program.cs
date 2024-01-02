using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.UseRabbitMQMessageHandler(builder.Configuration);
builder.Services.AddTransient((svc) =>
{
    var sqlConnectionString = builder.Configuration.GetConnectionString("WorkshopManagementCN");
    var dbContextOptions = new DbContextOptionsBuilder<WorkshopManagementDBContext>()
        .UseSqlServer(sqlConnectionString)
        .Options;
    var dbContext = new WorkshopManagementDBContext(dbContextOptions);

    DBInitializer.Initialize(dbContext);

    return dbContext;
});

builder.Services.AddHostedService<EventHandlerWorker>();

var host = builder.Build();
await host.RunAsync();