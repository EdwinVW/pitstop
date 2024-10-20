namespace Pitstop.RepairManagementAPI.DataAccess;

public class RepairManagementContext(DbContextOptions<RepairManagementContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<RepairOrders> RepairOrders { get; set; }
    public DbSet<VehicleParts> VehicleParts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Vehicle>().HasKey(entity => entity.LicenseNumber);
        builder.Entity<Vehicle>().ToTable("Vehicle");

        builder.Entity<Customer>().HasKey(entity => entity.CustomerId);
        builder.Entity<Customer>().ToTable("Customer");

        builder.Entity<RepairOrders>()
            .Property(ro => ro.TotalCost)
            .HasColumnType("decimal(18,2)");
        builder.Entity<RepairOrders>().HasKey(entity => entity.Id);
        builder.Entity<RepairOrders>().ToTable("RepairOrders");


        builder.Entity<VehicleParts>()
            .Property(vp => vp.Cost)
            .HasColumnType("decimal(18,2)");
        builder.Entity<VehicleParts>().HasKey(entity => entity.Id);
        builder.Entity<VehicleParts>().ToTable("VehicleParts");

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