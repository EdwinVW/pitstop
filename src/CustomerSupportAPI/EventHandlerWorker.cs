using Newtonsoft.Json.Linq;
using Pitstop.CustomerSupportAPI.Events;
using Pitstop.CustomerSupportAPI.Models;

namespace Pitstop.CustomerSupportAPI;

public class EventHandlerWorker : IHostedService, IMessageHandlerCallback
{
    private readonly CustomerSupportContext _context;
    private readonly IMessageHandler _messageHandler;

    public EventHandlerWorker(IMessageHandler messageHandler, CustomerSupportContext context)
    {
        _messageHandler = messageHandler;
        _context = context;
    }

    public void Start()
    {
        _messageHandler.Start(this);
    }

    public void Stop()
    {
        _messageHandler.Stop();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _messageHandler.Start(this);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _messageHandler.Stop();
        return Task.CompletedTask;
    }

    public async Task<bool> HandleMessageAsync(string messageType, string message)
    {
        Log.Information("Received message of type {MessageType}", messageType);
        
        var messageObject = MessageSerializer.Deserialize(message);
        try
        {
            switch (messageType)
            {
                case "RepairOrderRejected":
                    return await HandleAsync(messageObject.ToObject<RepairOrderRejected>());
            }
        }
        catch (Exception ex)
        {
            var messageId = messageObject.Property("MessageId") != null ? messageObject.Property("MessageId").Value<string>() : "[unknown]";
            Log.Error(ex, "Error while handling {MessageType} message with id {MessageId}", messageType, messageId);
        }

        // always acknowledge message - any errors need to be dealt with locally.
        return true;
    }

    private async Task<bool> HandleAsync(RepairOrderRejected @event)
    {
        Log.Information("Register Repair Order Rejection: {RejectOrderId}, {RejectedAt}, {RejectReason}",
            @event.RepairOrderId, @event.RejectedAt, @event.RejectReason);

        try
        {
            await _context.Rejections.AddAsync(new Rejection(
                @event.RepairOrderId,
                @event.RejectReason,
                @event.RejectedAt
            ));
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            Console.WriteLine($"Skipped adding rejection with id: {@event.RepairOrderId}.");
        }

        return true;
    }
}