namespace Pitstop.VehicleManagement.DataAccess;

public class VehicleManagementDBContext : DbContext
{
    public VehicleManagementDBContext(DbContextOptions<VehicleManagementDBContext> options) : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Vehicle>().HasKey(m => m.LicenseNumber);
        builder.Entity<Vehicle>().ToTable("Vehicle");
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