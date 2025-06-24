namespace Pitstop.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ implementation of the MessagePublisher.
/// </summary>
public sealed class RabbitMQMessagePublisher : IMessagePublisher, IDisposable
{
    private const int DEFAULT_PORT = 5672;
    private const string DEFAULT_VIRTUAL_HOST = "/";
    
    private readonly List<string> _hosts;    
    private readonly string _virtualHost;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly string _exchange;
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMQMessagePublisher(string host, string username, string password, string exchange, int port)
    : this(new List<string>() { host }, DEFAULT_VIRTUAL_HOST, username, password, exchange, port)
    {
    }

    public RabbitMQMessagePublisher(string host, string virtualHost, string username, string password, string exchange)
        : this(new List<string>() { host }, virtualHost, username, password, exchange, DEFAULT_PORT)
    {
    }

    public RabbitMQMessagePublisher(string host, string virtualHost, string username, string password, string exchange, int port)
        : this(new List<string>() { host }, virtualHost, username, password, exchange, port)
    {
    }

    public RabbitMQMessagePublisher(string host, string username, string password, string exchange)
        : this(new List<string>() { host }, DEFAULT_VIRTUAL_HOST, username, password, exchange, DEFAULT_PORT)
    {
    }  

    public RabbitMQMessagePublisher(IEnumerable<string> hosts, string username, string password, string exchange)
        : this(hosts, DEFAULT_VIRTUAL_HOST, username, password, exchange, DEFAULT_PORT)
    {
    }

    public RabbitMQMessagePublisher(IEnumerable<string> hosts, string virtualHost, string username, string password, string exchange, int port)
    {
        _hosts = [.. hosts];
        _port = port;
        _virtualHost = virtualHost;
        _username = username;
        _password = password;
        _exchange = exchange;

        var logMessage = new StringBuilder();
        logMessage.AppendLine("Create RabbitMQ message-publisher instance using config:");
        logMessage.AppendLine($" - Hosts: {string.Join(',', _hosts.ToArray())}");
        logMessage.AppendLine($" - VirtualHost: {_virtualHost}");
        logMessage.AppendLine($" - Port: {_port}");
        logMessage.AppendLine($" - UserName: {_username}");
        logMessage.AppendLine($" - Password: {new string('*', _password.Length)}");
        logMessage.Append($" - Exchange: {_exchange}");
        Log.Information(logMessage.ToString());

        Connect();
    }

    /// <summary>
    /// Publish a message.
    /// </summary>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="routingKey">The routingkey to use (RabbitMQ specific).</param>
    public Task PublishMessageAsync(string messageType, object message, string routingKey)
    {
        return Task.Run(async () =>
            {
                string data = MessageSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(data);
                var properties = new BasicProperties
                {
                    Headers = new Dictionary<string, object> { { "MessageType", messageType } }
                };
                await _channel.BasicPublishAsync(_exchange, routingKey, false, properties, body);
            });
    }

    private void Connect()
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Log.Error("Error connecting to RabbitMQ. Retrying in 5 sec."); })
            .Execute(async () =>
            {
                var factory = new ConnectionFactory() { VirtualHost = _virtualHost, UserName = _username, Password = _password, Port = _port };
                factory.AutomaticRecoveryEnabled = true;
                _connection = await factory.CreateConnectionAsync(_hosts);
                _channel = await _connection.CreateChannelAsync();
                await _channel.ExchangeDeclareAsync(_exchange, "fanout", durable: true, autoDelete: false);
            });
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _channel = null;
        _connection?.Dispose();
        _connection = null;
    }

    ~RabbitMQMessagePublisher()
    {
        Dispose();
    }
}