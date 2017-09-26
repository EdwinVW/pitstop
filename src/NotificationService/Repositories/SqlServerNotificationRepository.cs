using System;
using System.Threading.Tasks;
using Dapper;
using Pitstop.NotificationService.Model;
using Polly;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace Pitstop.NotificationService.Repositories
{
    public class SqlServerNotificationRepository : INotificationRepository
    {
        private string _connectionString;

        public SqlServerNotificationRepository(string connectionString)
        {
            _connectionString = connectionString;

            // init db
            Policy
            .Handle<Exception>()
            .WaitAndRetry(5, r => TimeSpan.FromSeconds(5), (ex, ts) => { Console.WriteLine("Error connecting to DB. Retrying in 5 sec."); })
            .Execute(() => InitializeDB());
        }

        private async void InitializeDB()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString.Replace("Notification", "master")))
            {
                conn.Open();

                // create database
                string sql =
                    "IF DB_ID('Notification') IS NULL CREATE DATABASE Notification;";

                await conn.ExecuteAsync(sql);

                // create tables
                conn.ChangeDatabase("Notification");

                sql = "IF OBJECT_ID('Customer') IS NULL " +
                      "CREATE TABLE Customer (" +
                      "  CustomerId varchar(50) NOT NULL," +
                      "  Name varchar(50) NOT NULL," +
                      "  TelephoneNumber varchar(50)," +
                      "  EmailAddress varchar(50)," +
                      "  PRIMARY KEY(CustomerId));" +

                      "IF OBJECT_ID('MaintenanceJob') IS NULL " +
                      "CREATE TABLE MaintenanceJob (" +
                      "  JobId varchar(50) NOT NULL," +
                      "  LicenseNumber varchar(50) NOT NULL," +
                      "  CustomerId varchar(50) NOT NULL," +
                      "  StartTime datetime2 NOT NULL," +
                      "  Description varchar(250) NOT NULL," +
                      "  PRIMARY KEY(JobId));";

                await conn.ExecuteAsync(sql);
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

        public async Task RegisterMaintenanceJobAsync(MaintenanceJob job)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql =
                    "insert into MaintenanceJob(JobId, LicenseNumber, CustomerId, StartTime, Description) " +
                    "values(@JobId, @LicenseNumber, @CustomerId, @StartTime, @Description);";
                await conn.ExecuteAsync(sql, job);
            }
        }

        public async Task RegisterCustomerAsync(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql =
                    "insert into Customer(CustomerId, Name, TelephoneNumber, EmailAddress) " +
                    "values(@CustomerId, @Name, @TelephoneNumber, @EmailAddress);";
                await conn.ExecuteAsync(sql, customer);
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetMaintenanceJobsForTodayAsync(DateTime date)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryAsync<MaintenanceJob>(
                    "select * from MaintenanceJob where StartTime >= @Today and StartTime < @Tomorrow",
                    new { Today = date.Date, Tomorrow = date.AddDays(1).Date });
            }
        }

        public async Task RemoveMaintenanceJobsAsync(IEnumerable<string> jobIds)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql =
                    "delete MaintenanceJob " +
                    "where JobId = @JobId;";
                await conn.ExecuteAsync(sql, jobIds.Select(j => new { JobId = j }));
            }
        }
    }
}
