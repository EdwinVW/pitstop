namespace Pitstop.Infrastructure.Messaging;

public interface IMessageHandler
{
    void Start(IMessageHandlerCallback callback);
    void Stop();
}