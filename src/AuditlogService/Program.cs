using Microsoft.Extensions.Configuration;
using Pitstop.Infrastructure.Messaging;
using Serilog;
using System;
using System.IO;
using System.Threading;

namespace AuditlogService
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
            // get configuration
            var rabbitMQConfigSection = Config.GetSection("RabbitMQ");
            string host = rabbitMQConfigSection["Host"];
            string userName = rabbitMQConfigSection["UserName"];
            string password = rabbitMQConfigSection["Password"];

            var auditlogConfigSection = Config.GetSection("Auditlog");
            string logPath = auditlogConfigSection["path"];

            // start auditlog manager
            RabbitMQMessageHandler messageHandler = new RabbitMQMessageHandler(host, userName, password, "Pitstop", "Auditlog", "");
            AuditLogManager manager = new AuditLogManager(messageHandler, logPath);
            manager.Start();

            if (_env == "Development")
            {
                Console.WriteLine("Auditlog service started. Press any key to stop...");
                Console.ReadKey(true);
                manager.Stop();
            }
            else
            {
                Log.Information("AuditLog service started.");

                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }
}