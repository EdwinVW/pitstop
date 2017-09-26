using Microsoft.Extensions.Configuration;
using Pitstop.Infrastructure.Messaging;
using System;
using System.IO;
using System.Threading;

namespace Pitstop.TimeService
{
    class Program
    {
        private static string _env;
        public static IConfigurationRoot Config { get; private set; }

        static Program()
        {
            _env = Environment.GetEnvironmentVariable("PITSTOP_ENVIRONMENT");

            Console.WriteLine($"Environment: {_env}");

            Config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{_env}.json", optional: false)
                .Build();
        }

        static void Main(string[] args)
        {
            // get configuration
            var configSection = Config.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];

            // start time manager
            RabbitMQMessagePublisher messagePublisher = new RabbitMQMessagePublisher(host, userName, password, "Pitstop");
            TimeManager manager = new TimeManager(messagePublisher);
            manager.Start();

            if (_env == "Development")
            {
                Console.WriteLine("Time service started. Press any key to stop...");
                Console.ReadKey(true);
                manager.Stop();
            }
            else
            {
                Console.WriteLine("Time service started.");
                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }
}