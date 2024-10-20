namespace Pitstop.RepairManagementAPI.DataAccess;

public static class DBInitializer
{
    public static void Initialize(RepairManagementContext context)
    {
        Log.Information("Ensure RepairManagementAPI Database");

        Policy
            .Handle<Exception>()
            .WaitAndRetry(5, r => TimeSpan.FromSeconds(5), (ex, ts) => { Log.Error("Error connecting to DB. Retrying in 5 sec."); })
            .Execute(() => context.Database.Migrate());

        Log.Information("RepairManagementAPI Database available");
    }
}