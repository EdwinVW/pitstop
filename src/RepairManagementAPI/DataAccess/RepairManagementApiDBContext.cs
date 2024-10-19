using Microsoft.EntityFrameworkCore;
using Pitstop.RepairManagementAPI.Model;

namespace Pitstop.RepairManagementAPI.DataAccess;

public class RepairManagementApiDBContext : DbContext
{
    public RepairManagementApiDBContext()
    {
    }

    public RepairManagementApiDBContext(DbContextOptions<RepairManagementApiDBContext> options) : base(options)
    {
    }

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

        builder.Entity<RepairOrders>().HasKey(entity => entity.Id);
        builder.Entity<RepairOrders>().ToTable("RepairOrders");

        builder.Entity<VehicleParts>().HasKey(entity => entity.Id);
        builder.Entity<VehicleParts>().ToTable("VehicleParts");

        base.OnModelCreating(builder);
    }
}