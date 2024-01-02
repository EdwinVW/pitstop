using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;

var builder = DistributedApplication.CreateBuilder(args);

var databaseServer = builder.AddSqlServerContainer("pitstop_database");
var customerManagementDb = databaseServer.AddDatabase("CustomerManagement");
var eventStoreDb = databaseServer.AddDatabase("EventStore");
var invoicingDb = databaseServer.AddDatabase("Invoicing");
var notificationDb = databaseServer.AddDatabase("Notification");
var vehicleManagementDb = databaseServer.AddDatabase("VehicleManagement");
var workshopManagementDb = databaseServer.AddDatabase("WorkshopManagement");

builder.AddProject<Projects.Pitstop_AuditlogService>("AuditLogService");

builder.AddProject<Projects.Pitstop_CustomerManagementAPI>("CustomerManagementAPI")
    .WithLaunchProfile("CustomerManagementAPI")
    .WithReference(customerManagementDb);

builder.AddProject<Projects.Pitstop_InvoiceService>("InvoiceService")
    .WithReference(invoicingDb);

builder.AddProject<Projects.Pitstop_NotificationService>("NotificationService")
    .WithReference(notificationDb);

builder.AddProject<Projects.Pitstop_TimeService>("TimeService");

builder.AddProject<Projects.Pitstop_VehicleManagementAPI>("VehicleManagementAPI")
    .WithLaunchProfile("VehicleManagementAPI")
    .WithReference(vehicleManagementDb);

builder.AddProject<Projects.Pitstop_WebApp>("WebApp");

builder.AddProject<Projects.Pitstop_WorkshopManagementAPI>("WorkshopManagementAPI")
    .WithLaunchProfile("WorkshopManagementAPI")
    .WithReference(eventStoreDb)
    .WithReference(workshopManagementDb);

builder.AddProject<Projects.Pitstop_WorkshopManagementEventHandler>("WorkshopManagementEventHandler")
    .WithReference(workshopManagementDb);


var host = builder.Build();
await host.RunAsync();
