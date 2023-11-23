

using BookingApi.Models;
using BookingApi.Persistance;
using Microsoft.VisualBasic;
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
         private IFlightInfoRepository flightrepo;

        public RabbitMQChannel(RabbitMQConnection connection, IFlightInfoRepository flightInfo)
        {
            var channel = connection.CreateChannel().Result;
            this._channel = channel;
            CreateDefaultDeadLetterQueue();
            flightrepo = flightInfo;
        }

        public void CreateExchange(string exchangeName, string type = "")
        {
            type = (type == default && type == "") ? type : ExchangeType.Topic;
            var name = exchangeName;
            _channel.ExchangeDeclare(exchange: name, type: type);

            
        }

        public void BindQueueToChannel(string exchange, string queueName, string routingKey = default)
        {
            routingKey = routingKey ?? string.Empty;
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


        public void CreateQueue(string queueName = default,
                                 bool durable = false,
                                 bool exclusive = false,
                                 bool autodelete = false,
                                 Dictionary<string, object>? arguments = null, bool doesContainDeadletter = true)
        {
            if (doesContainDeadletter == true) arguments = _defaultDeadLetterQueue;
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
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var flight = JsonConvert.DeserializeObject<FlightInfoModel>(body);
                if(flight != null)
                {
                    flightrepo.Add(flight);
                }
            };

            _channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer);
        }



        public void PublishMessagesToExchange(string exchangeName, 
                                              object msg, 
                                              string routingKey = "", 
                                              IBasicProperties properties = null, 
                                              string exchangeType = ExchangeType.Topic)
        {

            
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));
            
            if(exchangeName == string.Empty){
                // CreateQueue(queueName: routingKey, doesContainDeadletter: true);
                
            }
            else{
                CreateExchange(exchangeName, exchangeType);
            }
            
            _channel.BasicPublish(exchange: exchangeName,
                                   routingKey: routingKey,
                                   mandatory: false,
                                   basicProperties: properties,
                                   body: body);
                                   
        }
    }
}
