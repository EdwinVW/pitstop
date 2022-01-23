namespace Pitstop.Infrastructure.Messaging;

public class Message
{
    public readonly Guid MessageId;
    public readonly string MessageType;

    public Message() : this(Guid.NewGuid())
    {
    }

    public Message(Guid messageId)
    {
        MessageId = messageId;
        MessageType = this.GetType().Name;
    }

    public Message(string messageType) : this(Guid.NewGuid())
    {
        MessageType = messageType;
    }

    public Message(Guid messageId, string messageType)
    {
        MessageId = messageId;
        MessageType = messageType;
    }
}