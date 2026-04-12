IF DB_ID('CustomerManagement') IS NOT NULL delete from CustomerManagement.dbo.Customer;

IF DB_ID('Invoicing') IS NOT NULL delete from Invoicing.dbo.Invoice;
IF DB_ID('Invoicing') IS NOT NULL delete from Invoicing.dbo.Customer;
IF DB_ID('Invoicing') IS NOT NULL delete from Invoicing.dbo.MaintenanceJob;

IF DB_ID('Notification') IS NOT NULL delete from Notification.dbo.Customer;
IF DB_ID('Notification') IS NOT NULL delete from Notification.dbo.MaintenanceJob;

IF DB_ID('VehicleManagement') IS NOT NULL delete from VehicleManagement.dbo.Vehicle;

IF DB_ID('WorkshopManagement') IS NOT NULL delete from WorkshopManagement.dbo.MaintenanceJob;
IF DB_ID('WorkshopManagement') IS NOT NULL delete from WorkshopManagement.dbo.Vehicle;
IF DB_ID('WorkshopManagement') IS NOT NULL delete from WorkshopManagement.dbo.Customer;

IF DB_ID('WorkshopManagementEventStore') IS NOT NULL delete from WorkshopManagementEventStore.dbo.WorkshopPlanningEvent;
IF DB_ID('WorkshopManagementEventStore') IS NOT NULL delete from WorkshopManagementEventStore.dbo.WorkshopPlanning;
