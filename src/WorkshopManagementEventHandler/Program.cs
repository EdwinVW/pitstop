using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementEventHandler.DataAccess;
using Polly;
using Serilog;
using System;
using System.IO;
using System.Threading;

namespace Pitstop.WorkshopManagementEventHandler
{
    class Program
    {
        private static string _env;
        public static IConfigurationRoot Config { get; private set; }

        static Program()
        {
            _env = Environment.GetEnvironmentVariable("PITSTOP_ENVIRONMENT");

            Config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{_env}.json", optional: false)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Config)
                .CreateLogger();

            Log.Information($"Environment: {_env}");
        }

        static void Main(string[] args)
        {
            Startup();
        }

        private static void Startup()
        {
            // setup RabbitMQ
            var configSection = Config.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];

            // setup messagehandler
            RabbitMQMessageHandler messageHandler = new RabbitMQMessageHandler(host, userName, password, "Pitstop", "WorkshopManagement", "");

            // setup DBContext
            var sqlConnectionString = Config.GetConnectionString("WorkshopManagementCN");
            var dbContextOptions = new DbContextOptionsBuilder<WorkshopManagementDBContext>()
                .UseSqlServer(sqlConnectionString)
                .Options;
            var dbContext = new WorkshopManagementDBContext(dbContextOptions);

            Policy
                .Handle<Exception>()
                .WaitAndRetry(5, r => TimeSpan.FromSeconds(5), (ex, ts) => { Log.Error("Error connecting to DB. Retrying in 5 sec."); })
                .Execute(() => DBInitializer.Initialize(dbContext));

            // start event-handler
            EventHandler eventHandler = new EventHandler(messageHandler, dbContext);
            eventHandler.Start();

            if (_env == "Development")
            {
                Log.Information("WorkshopManagement Eventhandler started.");
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
                eventHandler.Stop();
            }
            else
            {
                Log.Information("WorkshopManagement Eventhandler started.");
                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }
}