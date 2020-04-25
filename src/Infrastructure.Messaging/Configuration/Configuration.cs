using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pitstop.Infrastructure.Messaging;

namespace Pitstop.Infrastructure.Messaging.Configuration
{
    public static class Configuration
    {
        private static string _host;
        private static string _userName;
        private static string _password;
        private static string _exchange;
        private static string _queue;
        private static string _routingKey;

        public static void UseRabbitMQMessageHandler(this IServiceCollection services, IConfiguration config)
        {
            GetRabbitMQSettings(config, "RabbitMQHandler");
            services.AddTransient<IMessageHandler>(_ => new RabbitMQMessageHandler(
                _host, _userName, _password, _exchange, _queue, _routingKey));
        }

        public static void UseRabbitMQMessagePublisher(this IServiceCollection services, IConfiguration config)
        {
            GetRabbitMQSettings(config, "RabbitMQPublisher");
            services.AddTransient<IMessagePublisher>(_ => new RabbitMQMessagePublisher(
                _host, _userName, _password, _exchange));
        }        

        private static void GetRabbitMQSettings(IConfiguration config, string sectionName)
        {
            bool valid = true;
            var errors = new List<string>();

            var configSection = config.GetSection(sectionName);
            if (!configSection.Exists())
            {
                throw new InvalidConfigurationException($"Required config-section '{sectionName}' not found.");
            }

            _host = configSection["Host"];
            if (string.IsNullOrEmpty(_host))
            {
                valid = false;
                errors.Add("Required config-setting 'Host' not found.");
            }

            _userName = configSection["UserName"];
            if (string.IsNullOrEmpty(_userName))
            {
                valid = false;
                errors.Add("Required config-setting 'UserName' not found.");
            }            

            _password = configSection["Password"];
            if (string.IsNullOrEmpty(_password))
            {
                valid = false;
                errors.Add("Required config-setting 'Password' not found.");
            } 

            _exchange = configSection["Exchange"];
            if (string.IsNullOrEmpty(_exchange))
            {
                valid = false;
                errors.Add("Required config-setting 'Exchange' not found.");
            } 

            if (sectionName == "RabbitMQHandler")
            {
                _queue = configSection["Queue"];
                if (string.IsNullOrEmpty(_queue))
                {
                    valid = false;
                    errors.Add("Required config-setting 'Queue' not found.");
                }                 
                _routingKey = configSection["RoutingKey"] ?? "";
            }

            if (!valid)
            {
                var errorMessage = new StringBuilder("Invalid RabbitMQ configuration:");
                errors.ForEach(e => errorMessage.AppendLine(e));
                throw new InvalidConfigurationException(errorMessage.ToString());
            }
        }
    }
}