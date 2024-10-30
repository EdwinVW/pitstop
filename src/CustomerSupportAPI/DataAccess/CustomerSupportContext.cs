namespace Pitstop.CustomerSupportAPI.DataAccess;

public class CustomerSupportContext(DbContextOptions<CustomerSupportContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    public void MigrateDB()
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(10, r => TimeSpan.FromSeconds(10))
            .Execute(() => Database.Migrate());
    }
}