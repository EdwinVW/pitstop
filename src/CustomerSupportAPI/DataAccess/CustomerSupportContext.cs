using Pitstop.CustomerSupportAPI.Models;

namespace Pitstop.CustomerSupportAPI.DataAccess;

public class CustomerSupportContext(DbContextOptions<CustomerSupportContext> options) : DbContext(options)
{
    public DbSet<Rejection> Rejections { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // This should use a better key
        builder.Entity<Rejection>().HasKey(r => r.RepairOrderId);
        builder.Entity<Rejection>().ToTable("Rejections");
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