using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddKeyedSqlServerClient("WorkshopManagementEventStore");
builder.AddKeyedSqlServerClient("WorkshopManagement");

// add repo
var eventStoreConnectionString = builder.Configuration.GetConnectionString("WorkshopManagementEventStore");
builder.Services.AddTransient<IEventSourceRepository<WorkshopPlanning>>((sp) =>
    new SqlServerWorkshopPlanningEventSourceRepository(eventStoreConnectionString));

var workshopManagementConnectionString = builder.Configuration.GetConnectionString("WorkshopManagement");
builder.Services.AddTransient<IVehicleRepository>((sp) => new SqlServerRefDataRepository(workshopManagementConnectionString));
builder.Services.AddTransient<ICustomerRepository>((sp) => new SqlServerRefDataRepository(workshopManagementConnectionString));


// add messagepublisher
builder.Services.UseRabbitMQMessagePublisher(builder.Configuration);

// add commandhandlers
builder.Services.AddCommandHandlers();

// Add framework services.
builder.Services
    .AddMvc((options) => options.EnableEndpointRouting = false)
    .AddNewtonsoftJson();

// Register the Swagger generator, defining one or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo  { Title = "WorkshopManagement API", Version = "v1" });
});

// Setup MVC
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMvc();
app.UseDefaultFiles();
app.UseStaticFiles();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WorkshopManagement API - v1");
});

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();