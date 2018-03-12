using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementEventHandler.DataAccess;
using Polly;
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
            _env = Environment.GetEnvironmentVariable("PITSTOP_ENVIRONMENT") ?? "Production";

            Console.WriteLine($"Environment: {_env}");

            Config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{_env}.json", optional: false)
                .Build();
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
                .WaitAndRetry(5, r => TimeSpan.FromSeconds(5), (ex, ts) => { Console.WriteLine("Error connecting to DB. Retrying in 5 sec."); })
                .Execute(() => DBInitializer.Initialize(dbContext));

            // start event-handler
            EventHandler eventHandler = new EventHandler(messageHandler, dbContext);
            eventHandler.Start();

            if (_env == "Development")
            {
                Console.WriteLine("WorkshopManagement EventHandler started. Press any key to stop...");
                Console.ReadKey(true);
                eventHandler.Stop();
            }
            else
            {
                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }
}