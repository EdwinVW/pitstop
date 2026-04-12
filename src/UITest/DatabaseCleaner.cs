using Microsoft.Data.SqlClient;

namespace Pitstop.UITest;

/// <summary>
/// Clears all Pitstop databases before each test to ensure a clean state.
/// SQL statements match src/scripts/ClearDatabases.sql.
/// </summary>
public static class DatabaseCleaner
{
  // Delete order is important: child rows before parent rows to avoid FK violations.
  private static readonly string[] _deleteStatements =
  [
        "IF DB_ID('CustomerManagement') IS NOT NULL delete from CustomerManagement.dbo.Customer",

        "IF DB_ID('Invoicing') IS NOT NULL delete from Invoicing.dbo.Invoice",
        "IF DB_ID('Invoicing') IS NOT NULL delete from Invoicing.dbo.Customer",
        "IF DB_ID('Invoicing') IS NOT NULL delete from Invoicing.dbo.MaintenanceJob",

        "IF DB_ID('Notification') IS NOT NULL delete from Notification.dbo.Customer",
        "IF DB_ID('Notification') IS NOT NULL delete from Notification.dbo.MaintenanceJob",

        "IF DB_ID('VehicleManagement') IS NOT NULL delete from VehicleManagement.dbo.Vehicle",

        "IF DB_ID('WorkshopManagement') IS NOT NULL delete from WorkshopManagement.dbo.MaintenanceJob",
        "IF DB_ID('WorkshopManagement') IS NOT NULL delete from WorkshopManagement.dbo.Vehicle",
        "IF DB_ID('WorkshopManagement') IS NOT NULL delete from WorkshopManagement.dbo.Customer",

        "IF DB_ID('WorkshopManagementEventStore') IS NOT NULL delete from WorkshopManagementEventStore.dbo.WorkshopPlanningEvent",
        "IF DB_ID('WorkshopManagementEventStore') IS NOT NULL delete from WorkshopManagementEventStore.dbo.WorkshopPlanning",
    ];

  public static async Task ClearAllAsync()
  {
      await using var connection = new SqlConnection(TestConstants.SqlConnectionString);
      await connection.OpenAsync();

      foreach (var sql in _deleteStatements)
      {
        await using var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
      }

      await Task.Delay(TestConstants.DatabaseCleanupDelay); // allow time for the database cleanup to fully complete before starting the test        
  }
}
