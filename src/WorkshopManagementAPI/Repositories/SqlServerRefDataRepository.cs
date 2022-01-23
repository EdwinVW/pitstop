namespace Pitstop.WorkshopManagementAPI.Repositories;

using Pitstop.WorkshopManagementAPI.Repositories.Model;

public class SqlServerRefDataRepository : IVehicleRepository, ICustomerRepository
{
    private string _connectionString;

    public SqlServerRefDataRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        List<Customer> customers = new List<Customer>();
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            try
            {
                var customersSelection = await conn.QueryAsync<Customer>("select * from Customer");

                if (customersSelection != null)
                {
                    customers.AddRange(customersSelection);
                }
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
            }
        }

        return customers;
    }


    public async Task<IEnumerable<Model.Vehicle>> GetVehiclesAsync()
    {
        List<Vehicle> vehicles = new List<Vehicle>();
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            try
            {
                var vehicleSelection = await conn.QueryAsync<Vehicle>("select * from Vehicle");

                if (vehicleSelection != null)
                {
                    vehicles.AddRange(vehicleSelection);
                }
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
            }
        }

        return vehicles;
    }

    public async Task<Vehicle> GetVehicleAsync(string licenseNumber)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            try
            {
                return await conn.QueryFirstOrDefaultAsync<Vehicle>("select * from Vehicle where LicenseNumber = @LicenseNumber",
                    new { LicenseNumber = licenseNumber });

            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
            }
            return null;
        }
    }

    public async Task<Customer> GetCustomerAsync(string customerId)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            try
            {
                return await conn.QueryFirstOrDefaultAsync<Customer>("select * from Customer where CustomerId = @CustomerId",
                    new { CustomerId = customerId });
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
            }
            return null;
        }
    }


    private static void HandleSqlException(SqlException ex)
    {
        if (ex.Errors.Count > 0)
        {
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                if (ex.Errors[i].Number == 4060)
                {
                    throw new DatabaseNotCreatedException("WorkshopManagement database not found. This database is automatically created by the WorkshopManagementEventHandler. Run this service first.");
                }
            }
        }

        // rethrow original exception without poluting the stacktrace
        ExceptionDispatchInfo.Capture(ex).Throw();
    }
}