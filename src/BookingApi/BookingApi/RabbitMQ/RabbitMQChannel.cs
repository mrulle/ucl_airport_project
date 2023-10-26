

using BookingApi.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;
using System.Threading.Channels;
using System.Xml.Linq;

namespace BookingApi.RabbitMQ
{
    public class RabbitMQChannel
    {
        private readonly IModel _channel;
        private List<string> _routingKeys = new List<string>();
        private List<string> _queues = new List<string>();
        private Dictionary<string, object> _defaultDeadLetterQueue = new Dictionary<string, object>();
        private string _defaultExchange = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_EXCHANGE") ?? "FlightJourney";
        private string _defaultQueueName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_QUEUENAME") ?? "Booking";
        private string _defaultRoutingKey = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_ROUTINGKEY") ?? "BookingPlaneAndFlight";

        public RabbitMQChannel(RabbitMQConnection connection)
        {
            var channel = connection.CreateChannel().Result;
            this._channel = channel;
            CreateDefaultDeadLetterQueue();
        }

        public void CreateExchange(string exchangeName, string type = "")
        {
            type = (type == default && type == "") ? type : ExchangeType.Topic;
            var name = exchangeName ?? _defaultExchange;
            _channel.ExchangeDeclare(exchange: name, type: type);

            
        }

        public void BindQueueToChannel(string exchange, string queueName, string routingKey = default)
        {
            exchange = exchange ?? _defaultExchange;
            queueName = queueName ?? _defaultQueueName;
            routingKey = routingKey ?? _defaultQueueName;
            CreateExchange(exchange);
            _routingKeys.Add(routingKey);
            _channel.QueueBind(
                queue: queueName,
                exchange: exchange,
                routingKey: routingKey);


        }

        private void CreateDefaultDeadLetterQueue()
        {
            CreateExchange("default-dead-letter-exchange", ExchangeType.Direct);
            _defaultDeadLetterQueue.Add("x-dead-letter-exchange", "default-dead-letter-exchange");

            CreateQueue("dead-Letter-Queue", doesContainDeadletter: false);
            Thread.Sleep(100);
            BindQueueToChannel("default-dead-letter-exchange", "dead-Letter-Queue");
        }


        private void CreateQueue(string queueName = default,
                                 bool durable = false,
                                 bool exclusive = false,
                                 bool autodelete = false,
                                 Dictionary<string, object>? arguments = null, bool doesContainDeadletter = true)
        {
            if (doesContainDeadletter == true) arguments = _defaultDeadLetterQueue;
            queueName = queueName ?? _defaultQueueName;

            bool noqueue = true;
            while(noqueue)
            {
                try
                {
                    if (doesContainDeadletter)
                    {
                        _channel.QueueDeclare(queueName, durable, exclusive, autodelete, arguments);
                    }
                    else
                    {
                        _channel.QueueDeclare(queueName, durable, exclusive, autodelete, null);
                    }
                    noqueue = false;
                }
                catch (Exception ex)
                {

                }
            }
        }
        public void ConsumeMessagesFromChannel(string queueName, string routingKey = "")
        {
            queueName = queueName ?? _defaultQueueName;
            routingKey = routingKey ?? _defaultRoutingKey;
            var key = _routingKeys.Find(key => key == routingKey) ?? throw new NullReferenceException("No routingKeys Exists with that value");
            var name = _routingKeys.Find(name => name == queueName) ?? throw new NullReferenceException("No queueName Exists with that value");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var msg = JsonConvert.DeserializeObject<FlightInfoModel>(body);
                // TODO:: Save the data 
            };

            _channel.BasicConsume(
                queue: name,
                autoAck: true,
                consumer: consumer);

        }
    }
}
