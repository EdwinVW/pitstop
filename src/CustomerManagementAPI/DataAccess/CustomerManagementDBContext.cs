using Microsoft.EntityFrameworkCore;
using Pitstop.CustomerManagementAPI.Model;

namespace Pitstop.CustomerManagementAPI.DataAccess
{
    public class CustomerManagementDBContext : DbContext
    {
        public CustomerManagementDBContext(DbContextOptions<CustomerManagementDBContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>().HasKey(m => m.CustomerId);
            builder.Entity<Customer>().ToTable("Customer");
            base.OnModelCreating(builder);
        }
    }
}
