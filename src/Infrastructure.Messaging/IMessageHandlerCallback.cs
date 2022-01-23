namespace Pitstop.Infrastructure.Messaging;

public interface IMessageHandlerCallback
{
    Task<bool> HandleMessageAsync(string messageType, string message);
}