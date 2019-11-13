using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Pitstop.Infrastructure.Messaging;

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
            return Task.FromResult(false);
        }
    }
}