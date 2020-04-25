using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using Polly;
using Serilog;
using System.Collections.Generic;
using System.Linq;

namespace Pitstop.Infrastructure.Messaging
{
    public class RabbitMQMessageHandler : IMessageHandler
    {
        private readonly List<string> _hosts;
        private readonly string _username;
        private readonly string _password;
        private readonly string _exchange;
        private readonly string _queuename;
        private readonly string _routingKey;
        private IConnection _connection;
        private IModel _model;
        private AsyncEventingBasicConsumer _consumer;
        private string _consumerTag;
        private IMessageHandlerCallback _callback;

        public RabbitMQMessageHandler(string host, string username, string password, string exchange, string queuename, string routingKey)
            : this(new List<string>() { host }, username, password, exchange, queuename, routingKey)
        {
        }

        public RabbitMQMessageHandler(IEnumerable<string> hosts, string username, string password, string exchange, string queuename, string routingKey)
        {
            _hosts = new List<string>(hosts);
            _username = username;
            _password = password;
            _exchange = exchange;
            _queuename = queuename;
            _routingKey = routingKey;

            var logMessage = new StringBuilder();
            logMessage.AppendLine("Create RabbitMQ message-handler instance using config:");
            logMessage.AppendLine($" - Hosts: {string.Join(',', _hosts.ToArray())}");
            logMessage.AppendLine($" - UserName: {_username}");
            logMessage.AppendLine($" - Password: {new string('*', _password.Length)}");
            logMessage.AppendLine($" - Exchange: {_exchange}");
            logMessage.AppendLine($" - Queue: {_queuename}");
            logMessage.Append($" - RoutingKey: {_routingKey}");
            Log.Information(logMessage.ToString());
        }        

        public void Start(IMessageHandlerCallback callback)
        {
            _callback = callback;

            Policy
                .Handle<Exception>()
                .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Log.Error("Error connecting to RabbitMQ. Retrying in 5 sec."); })
                .Execute(() =>
                {
                    var factory = new ConnectionFactory() { UserName = _username, Password = _password, DispatchConsumersAsync = true };
                    _connection = factory.CreateConnection(_hosts);
                    _model = _connection.CreateModel();
                    _model.ExchangeDeclare(_exchange, "fanout", durable: true, autoDelete: false);
                    _model.QueueDeclare(_queuename, durable: true, autoDelete: false, exclusive: false);
                    _model.QueueBind(_queuename, _exchange, _routingKey);
                    _consumer = new AsyncEventingBasicConsumer(_model);
                    _consumer.Received += Consumer_Received;
                    _consumerTag = _model.BasicConsume(_queuename, false, _consumer);
                });
        }

        public void Stop()
        {
            _model.BasicCancel(_consumerTag);
            _model.Close(200, "Goodbye");
            _connection.Close();
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            if (await HandleEvent(ea))
            {
                _model.BasicAck(ea.DeliveryTag, false);
            }
        }

        private Task<bool> HandleEvent(BasicDeliverEventArgs ea)
        {
            // determine messagetype
            string messageType = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["MessageType"]);

            // get body
            string body = Encoding.UTF8.GetString(ea.Body.ToArray());

            // call callback to handle the message
            return _callback.HandleMessageAsync(messageType, body);
        }
    }
}
