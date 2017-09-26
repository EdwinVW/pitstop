using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Pitstop.Infrastructure.Messaging
{
    public class RabbitMQMessageHandler : IMessageHandler
    {
        private string _host;
        private string _username;
        private string _password;
        private string _exchange;
        private string _queuename;
        private string _routingKey;
        private IConnection _connection;
        private IModel _model;
        private EventingBasicConsumer _consumer;
        private string _consumerTag;
        private IMessageHandlerCallback _callback;

        public RabbitMQMessageHandler(string host, string username, string password, string exchange, string queuename, string routingKey)
        {
            _host = host;
            _username = username;
            _password = password;
            _exchange = exchange;
            _queuename = queuename;
            _routingKey = routingKey;
        }

        public void Start(IMessageHandlerCallback callback)
        {
            _callback = callback;

            Policy
                .Handle<Exception>()
                .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Console.WriteLine("Error connecting to RabbitMQ. Retrying in 5 sec."); })
                .Execute(() =>
                {
                    var factory = new ConnectionFactory() { HostName = _host, UserName = _username, Password = _password };
                    _connection = factory.CreateConnection();
                    _model = _connection.CreateModel();
                    _model.ExchangeDeclare(_exchange, "fanout", durable: true, autoDelete: false);
                    _model.QueueDeclare(_queuename, durable: true, autoDelete: false, exclusive: false);
                    _model.QueueBind(_queuename, _exchange, _routingKey);
                    _consumer = new EventingBasicConsumer(_model);
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

        private async void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            if (await HandleEvent(ea))
            {
                _model.BasicAck(ea.DeliveryTag, false);
            }
        }

        private Task<bool> HandleEvent(BasicDeliverEventArgs ea)
        {
            // determine messagetype
            string messageTypeString = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["MessageType"]);
            MessageTypes messageType = MessageTypes.Unknown;
            Enum.TryParse<MessageTypes>(messageTypeString, out messageType);

            // get body
            string body = Encoding.UTF8.GetString(ea.Body);

            // call callback to handle the message
            return _callback.HandleMessageAsync(messageType, body);
        }
    }
}
