using Microsoft.Extensions.Configuration;
using Pitstop.Infrastructure.Messaging;
using Pitstop.InvoiceService.CommunicationChannels;
using Pitstop.InvoiceService.Repositories;
using Serilog;
using System;
using System.IO;
using System.Threading;

namespace Pitstop.InvoiceService
{
    class Program
    {
        private static string _env;
        public static IConfigurationRoot Config { get; private set; }

        static Program()
        {
            _env = Environment.GetEnvironmentVariable("PITSTOP_ENVIRONMENT") ?? "Production";

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
            var configSection = Config.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];

            var sqlConnectionString = Config.GetConnectionString("InvoiceServiceCN");

            var mailConfigSection = Config.GetSection("Email");
            string mailHost = mailConfigSection["Host"];
            int mailPort = Convert.ToInt32(mailConfigSection["Port"]);
            string mailUserName = mailConfigSection["User"];
            string mailPassword = mailConfigSection["Pwd"];

            // start invoice manager
            RabbitMQMessageHandler messageHandler = new RabbitMQMessageHandler(host, userName, password, "Pitstop", "Invoicing", "");
            IInvoiceRepository repo = new SqlServerInvoiceRepository(sqlConnectionString);
            IEmailCommunicator emailCommunicator = new SMTPEmailCommunicator(mailHost, mailPort, mailUserName, mailPassword);
            InvoiceManager manager = new InvoiceManager(messageHandler, repo, emailCommunicator);
            manager.Start();

            if (_env == "Development")
            {
                Log.Information("Invoice service started.");
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
                manager.Stop();
            }
            else
            {
                Log.Information("Invoice service started.");
                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }
}