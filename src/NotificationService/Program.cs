using Microsoft.Extensions.Configuration;
using Pitstop.Infrastructure.Messaging;
using Pitstop.NotificationService.NotificationChannels;
using Pitstop.NotificationService.Repositories;
using Serilog;
using System;
using System.IO;
using System.Threading;

namespace Pitstop.NotificationService
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
            var rmqConfigSection = Config.GetSection("RabbitMQ");
            string rmqHost = rmqConfigSection["Host"];
            string rmqUserName = rmqConfigSection["UserName"];
            string rmqPassword = rmqConfigSection["Password"];

            var mailConfigSection = Config.GetSection("Email");
            string mailHost = mailConfigSection["Host"];
            int mailPort = Convert.ToInt32(mailConfigSection["Port"]);
            string mailUserName = mailConfigSection["User"];
            string mailPassword = mailConfigSection["Pwd"];

            var sqlConnectionString = Config.GetConnectionString("NotificationServiceCN");

            // start notification service
            RabbitMQMessageHandler messageHandler = 
                new RabbitMQMessageHandler(rmqHost, rmqUserName, rmqPassword, "Pitstop", "Notifications", "");
            INotificationRepository repo = new SqlServerNotificationRepository(sqlConnectionString);
            IEmailNotifier emailNotifier = new SMTPEmailNotifier(mailHost, mailPort, mailUserName, mailPassword);
            NotificationManager manager = new NotificationManager(messageHandler, repo, emailNotifier);
            manager.Start();

            if (_env == "Development")
            {
                Log.Information("Notification service started.");
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
                manager.Stop();
            }
            else
            {
                Log.Information("Notification service started.");
                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }
}