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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerSupport API", Version = "v1" });
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

// add DBContext
var sqlConnectionString = builder.Configuration.GetConnectionString("CustomerSupportCN");
builder.Services.AddDbContext<CustomerSupportContext>(options => options.UseSqlServer(sqlConnectionString));


// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<CustomerSupportContext>();
 
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerSupport API - v1");
});

// auto migrate db
using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    scope.ServiceProvider.GetService<CustomerSupportContext>().MigrateDB();
}

app.UseHealthChecks("/hc");

app.MapControllers();

app.Run();