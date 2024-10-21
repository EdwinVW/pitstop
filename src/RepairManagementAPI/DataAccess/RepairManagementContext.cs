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

        builder.Entity<VehicleParts>().HasData(
            new VehicleParts(Guid.NewGuid(), "Engine", 1200.00m),
            new VehicleParts(Guid.NewGuid(), "Brake Pads", 300.00m),
            new VehicleParts(Guid.NewGuid(), "Transmission", 2400.00m),
            new VehicleParts(Guid.NewGuid(), "Fuel Pump", 600.00m),
            new VehicleParts(Guid.NewGuid(), "Alternator", 450.00m),
            new VehicleParts(Guid.NewGuid(), "Radiator", 250.00m),
            new VehicleParts(Guid.NewGuid(), "Exhaust", 500.00m),
            new VehicleParts(Guid.NewGuid(), "Suspension", 800.00m),
            new VehicleParts(Guid.NewGuid(), "Battery", 200.00m),
            new VehicleParts(Guid.NewGuid(), "Starter Motor", 400.00m),
            new VehicleParts(Guid.NewGuid(), "Headlights", 150.00m),
            new VehicleParts(Guid.NewGuid(), "Taillights", 120.00m),
            new VehicleParts(Guid.NewGuid(), "Spark Plugs", 75.00m),
            new VehicleParts(Guid.NewGuid(), "Clutch", 850.00m),
            new VehicleParts(Guid.NewGuid(), "Air Filter", 35.00m),
            new VehicleParts(Guid.NewGuid(), "Oil Filter", 25.00m),
            new VehicleParts(Guid.NewGuid(), "Timing Belt", 320.00m),
            new VehicleParts(Guid.NewGuid(), "Water Pump", 400.00m),
            new VehicleParts(Guid.NewGuid(), "Fuel Injector", 550.00m),
            new VehicleParts(Guid.NewGuid(), "Dashboard", 650.00m),
            new VehicleParts(Guid.NewGuid(), "Steering Wheel", 220.00m),
            new VehicleParts(Guid.NewGuid(), "Shock Absorbers", 300.00m),
            new VehicleParts(Guid.NewGuid(), "Brake Calipers", 280.00m),
            new VehicleParts(Guid.NewGuid(), "Catalytic Converter", 700.00m),
            new VehicleParts(Guid.NewGuid(), "Muffler", 350.00m)
        );


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