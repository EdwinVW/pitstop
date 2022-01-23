namespace Pitstop.InvoiceService.Repositories
{
    public class SqlServerInvoiceRepository : IInvoiceRepository
    {
        private string _connectionString;

        public SqlServerInvoiceRepository(string connectionString)
        {
            _connectionString = connectionString;

            // init db
            Log.Information("Initialize Database");

            Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(10, r => TimeSpan.FromSeconds(10), (ex, ts) => { Log.Error("Error connecting to DB. Retrying in 10 sec."); })
            .ExecuteAsync(InitializeDBAsync)
            .Wait();
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
                    "insert into MaintenanceJob(JobId, LicenseNumber, CustomerId, Description, Finished, InvoiceSent) " +
                    "values(@JobId, @LicenseNumber, @CustomerId, @Description, 0, 0);";
                await conn.ExecuteAsync(sql, job);
            }
        }

        public async Task RegisterCustomerAsync(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql =
                    "insert into Customer(CustomerId, Name, Address, PostalCode, City) " +
                    "values(@CustomerId, @Name, @Address, @PostalCode, @City);";
                await conn.ExecuteAsync(sql, customer);
            }
        }

        public async Task MarkMaintenanceJobAsFinished(string jobId, DateTime startTime, DateTime endTime)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query =
                    "update MaintenanceJob " +
                    "set StartTime = @StartTime, " +
                    "    EndTime = @EndTime, " +
                    "    Finished = 1 " +
                    "where JobId = @JobId";
                await conn.ExecuteAsync(query, new { JobId = jobId, StartTime = startTime, EndTime = endTime });
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetMaintenanceJobsToBeInvoicedAsync()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query =
                    "select * from MaintenanceJob " +
                    "where Finished = 1 " +
                    "and InvoiceSent = 0";
                return await conn.QueryAsync<MaintenanceJob>(query);
            }
        }

        public async Task RegisterInvoiceAsync(Invoice invoice)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // persist invoice
                string sql =
                    "insert into Invoice(InvoiceId, InvoiceDate, CustomerId, Amount, Specification, JobIds) " +
                    "values(@InvoiceId, @InvoiceDate, @CustomerId, @Amount, @Specification, @JobIds);";
                await conn.ExecuteAsync(sql, invoice);

                // update jobs to indicate invoice sent
                var jobIds = invoice.JobIds.Split('|').Select(jobId => new { JobId = jobId });
                sql =
                    "update MaintenanceJob " +
                    "set InvoiceSent = 1 " +
                    "where JobId = @JobId ";
                await conn.ExecuteAsync(sql, jobIds);
            }
        }

        private async Task InitializeDBAsync()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString.Replace("Invoicing", "master")))
            {
                await conn.OpenAsync();

                // create database
                string sql =
                    "IF NOT EXISTS(SELECT * FROM master.sys.databases WHERE name='Invoicing') CREATE DATABASE Invoicing;";

                await conn.ExecuteAsync(sql);
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // create tables
                string sql = "IF OBJECT_ID('Customer') IS NULL " +
                      "CREATE TABLE Customer (" +
                      "  CustomerId varchar(50) NOT NULL," +
                      "  Name varchar(50) NOT NULL," +
                      "  Address varchar(50)," +
                      "  PostalCode varchar(50)," +
                      "  City varchar(50)," +
                      "  PRIMARY KEY(CustomerId));" +

                      "IF OBJECT_ID('MaintenanceJob') IS NULL " +
                      "CREATE TABLE MaintenanceJob (" +
                      "  JobId varchar(50) NOT NULL," +
                      "  LicenseNumber varchar(50) NOT NULL," +
                      "  CustomerId varchar(50) NOT NULL," +
                      "  Description varchar(250) NOT NULL," +
                      "  StartTime datetime2 NULL," +
                      "  EndTime datetime2 NULL," +
                      "  Finished bit NOT NULL," +
                      "  InvoiceSent bit NOT NULL," +
                      "  PRIMARY KEY(JobId));" +

                      "IF OBJECT_ID('Invoice') IS NULL " +
                      "CREATE TABLE Invoice (" +
                      "  InvoiceId varchar(50) NOT NULL," +
                      "  InvoiceDate datetime2 NOT NULL," +
                      "  CustomerId varchar(50) NOT NULL," +
                      "  Amount decimal(5,2) NOT NULL," +
                      "  Specification text," +
                      "  JobIds varchar(250)," +
                      "  PRIMARY KEY(InvoiceId));";

                await conn.ExecuteAsync(sql);
            }
        }        
    }
}
