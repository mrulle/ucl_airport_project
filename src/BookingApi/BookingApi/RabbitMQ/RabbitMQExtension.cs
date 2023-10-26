using RabbitMQ.Client;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Channels;

namespace BookingApi.RabbitMQ
{

    public class RabbitMiddleware
    {
        private readonly RequestDelegate _next;

        public RabbitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, RabbitMQChannel channel)
        {
            Console.WriteLine(context.Request.ToString());
            var defaultExchange = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_EXCHANGE") ?? "FlightJourney";
            var defaultQueueName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_QUEUENAME") ?? "Booking";
            var defaultRoutingKey = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_ROUTINGKEY") ?? "BookingPlaneAndFlight";
            Debug.WriteLine(defaultExchange);
            Debug.WriteLine(defaultQueueName);
            Debug.WriteLine(defaultRoutingKey);
            channel.CreateExchange(defaultExchange, ExchangeType.Topic);
            channel.CreateQueue(defaultQueueName);
            Thread.Sleep(1000);
            channel.BindQueueToChannel(defaultExchange, defaultQueueName, defaultRoutingKey);
            channel.ConsumeMessagesFromChannel(defaultQueueName, defaultRoutingKey);

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }

    public static class RabbitMiddlewareExtension
    {
        
        public static IApplicationBuilder UseRabbit(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RabbitMiddleware>();
        }
    }
}
