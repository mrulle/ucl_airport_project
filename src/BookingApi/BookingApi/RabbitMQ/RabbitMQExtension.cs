using RabbitMQ.Client;

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
            var defaultExchange = Environment.GetEnvironmentVariable("RABBITMQ_FLIGHT_EXCHANGE") ?? "FlightJourney";
            var defaultQueueName = Environment.GetEnvironmentVariable("RABBITMQ_FLIGHT_QUEUENAME") ?? "Booking";
            var defaultRoutingKey = Environment.GetEnvironmentVariable("RABBITMQ_FLIGHT_ROUTINGKEY") ?? "BookingPlaneAndFlight";
            
            channel.CreateExchange(defaultExchange, ExchangeType.Topic);
            channel.CreateQueue(defaultQueueName);
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
