using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// setup logging
builder.Host.UseSerilog((context, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(context.Configuration)
        .Enrich.WithMachineName()
);


// add messagepublisher
builder.Services.UseRabbitMQMessagePublisher(builder.Configuration);

// Add framework services
builder.Services
    .AddMvc(options => options.EnableEndpointRouting = false)
    .AddNewtonsoftJson();

// Register the Swagger generator, defining one or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RepairManagement API", Version = "v1" });
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

// add DBContext
var sqlConnectionString = builder.Configuration.GetConnectionString("RepairManagementCN");
builder.Services.AddDbContext<RepairManagementContext>(options => options.UseSqlServer(sqlConnectionString));


// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<RepairManagementContext>();
 
// setup MVC
builder.Services.AddControllers();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RepairManagement API - v1");
});

// auto migrate db
using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    scope.ServiceProvider.GetService<RepairManagementContext>().MigrateDB();
}

app.UseHealthChecks("/hc");

app.MapControllers();

app.Run();