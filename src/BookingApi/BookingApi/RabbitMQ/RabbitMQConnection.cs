using BookingApi.Persistance;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace BookingApi.RabbitMQ
{
    public class RabbitMQConnection
    {


        private readonly string _host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        private int _rabbitmqPort;
        private ConnectionFactory? _factory;
        private IConnection _connection;
        private IFlightInfoRepository flightrepo;
        private IModel channel;
        private bool isConnected;

        public RabbitMQConnection()
        {
            _rabbitmqPort = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT")) == 0 ? 5672 
                                            : int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"));      
            

            _factory = new ConnectionFactory {  HostName = _host,
                                                Port = _rabbitmqPort,
                                                 RequestedHeartbeat = TimeSpan.FromSeconds(16)
            };

            
        }

        private IConnection CreateConnection()
        {
            IConnection createdConnection = null;
            while (!isConnected)
            {
                try
                {
                    // https://www.rabbitmq.com/heartbeats.html <-- docs says values between 5 and 20 seconds are optimal
                    createdConnection = _factory.CreateConnection();
                    isConnected = true;
                    
                }
                catch (Exception e) { 
                }
                Thread.Sleep(1500);
            }
            return createdConnection;
        }

        private void OnConnectionLost(object? sender, EventArgs e)
        {
            isConnected = false;
            _connection = CreateConnection();
        }


        public async Task<IModel> CreateChannel()
        {
            _connection = CreateConnection();
            channel = _connection.CreateModel();
            return channel;
        }
    }
}
