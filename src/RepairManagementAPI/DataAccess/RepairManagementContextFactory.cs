using Microsoft.EntityFrameworkCore.Design;

namespace Pitstop.RepairManagementAPI.DataAccess;

public class RepairManagementContextFactory : IDesignTimeDbContextFactory<RepairManagementContext>
{
    public RepairManagementContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<RepairManagementContext>();
        var connectionString = configuration.GetConnectionString("RepairManagementCN");
        
        optionsBuilder.UseSqlServer(connectionString);
        
        return new RepairManagementContext(optionsBuilder.Options);
    }
}