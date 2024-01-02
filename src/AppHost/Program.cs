var builder = DistributedApplication.CreateBuilder(args);


builder.AddProject<Projects.Pitstop_AuditlogService>("auditlogservice");
builder.AddProject<Projects.Pitstop_CustomerManagementAPI>("customermanagementapi");
builder.AddProject<Projects.Pitstop_InvoiceService>("invoiceservice");
builder.AddProject<Projects.Pitstop_NotificationService>("notificationservice");
builder.AddProject<Projects.Pitstop_TimeService>("timerservice");
builder.AddProject<Projects.Pitstop_VehicleManagementAPI>("vehiclemanagementapi");
builder.AddProject<Projects.Pitstop_WebApp>("webapp");
builder.AddProject<Projects.Pitstop_WorkshopManagementAPI>("workshopmanagementapi");
builder.AddProject<Projects.Pitstop_WorkshopManagementEventHandler>("workshopmanagementeventhandler");

builder.Build().Run();
