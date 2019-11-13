using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Pitstop.Infrastructure.Messaging;
using Serilog;

namespace CustomerEventHandler
{
    public class CustomerManager : IHostedService, IMessageHandlerCallback
    {
        private readonly IMessageHandler messageHandler;

        public CustomerManager(IMessageHandler messageHandler)
        {
            this.messageHandler = messageHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            messageHandler.Stop();
            return Task.CompletedTask;
        }

        public Task<bool> HandleMessageAsync(string messageType, string message)
        {
            var messageObject = MessageSerializer.Deserialize(message);
            CustomerRegistered customerRegisteredEvent = null;

            try
            {
                customerRegisteredEvent = messageObject.ToObject<CustomerRegistered>();
            }
            catch (System.Exception ex)
            {
                string messageId = messageObject.Property("MessageId") != null ? messageObject.Property("MessageId").Value<string>() : "[unknown]";
                Log.Error(ex, "Error while handling {MessageType} message with id {MessageId}.", messageType, messageId);
            }

            if (customerRegisteredEvent != null)
            {
                var logMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff")}|";
                logMessage += $"User has been registered: {customerRegisteredEvent.CustomerId}:{customerRegisteredEvent.Name}{Environment.NewLine}";
                Console.WriteLine($"{logMessage}");
                Log.Information("{MessageType} - {Body}", messageType, message);
            }

            // ack message - try catch deals with errors
            return Task.FromResult(true);
        }
    }
}