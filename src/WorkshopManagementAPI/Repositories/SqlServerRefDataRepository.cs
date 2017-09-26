using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Pitstop.WorkshopManagementAPI.Repositories.Model;
using System.Data.SqlClient;

namespace Pitstop.WorkshopManagementAPI.Repositories
{
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
                var customersSelection = await conn.QueryAsync<Customer>("select * from Customer");

                if (customersSelection != null)
                {
                    customers.AddRange(customersSelection);
                }
            }

            return customers;
        }

        public async Task<IEnumerable<Model.Vehicle>> GetVehiclesAsync()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var vehicleSelection = await conn.QueryAsync<Vehicle>("select * from Vehicle");

                if (vehicleSelection != null)
                {
                    vehicles.AddRange(vehicleSelection);
                }
            }

            return vehicles;
        }

        public async Task<Vehicle> GetVehicleAsync(string licenseNumber)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<Vehicle>("select * from Vehicle where LicenseNumber = @LicenseNumber",
                    new { LicenseNumber = licenseNumber });
            }
        }

        public async Task<Customer> GetCustomerAsync(string customerId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<Customer>("select * from Customer where CustomerId = @CustomerId",
                    new { CustomerId = customerId });
            }
        }
    }
}
