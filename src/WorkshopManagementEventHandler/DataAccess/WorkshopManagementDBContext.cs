namespace Pitstop.WorkshopManagementEventHandler.DataAccess;

public class WorkshopManagementDBContext : DbContext
{
    public WorkshopManagementDBContext()
    { }

    public WorkshopManagementDBContext(DbContextOptions<WorkshopManagementDBContext> options) : base(options)
    { }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<MaintenanceJob> MaintenanceJobs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Vehicle>().HasKey(entity => entity.LicenseNumber);
        builder.Entity<Vehicle>().ToTable("Vehicle");

        builder.Entity<Customer>().HasKey(entity => entity.CustomerId);
        builder.Entity<Customer>().ToTable("Customer");

        builder.Entity<MaintenanceJob>().HasKey(entity => entity.Id);
        builder.Entity<MaintenanceJob>().ToTable("MaintenanceJob");

        base.OnModelCreating(builder);
    }
}