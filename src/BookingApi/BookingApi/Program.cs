using BookingApi.Persistance;
using BookingApi.RabbitMQ;
using Serilog;

namespace BookingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Http(requestUri: "http://logstash:8080", queueLimit: null)
                .CreateLogger();

            builder.Host.UseSerilog();

            var environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if ( environment is null ) {
                environment = "Development";
            }
            Console.WriteLine($"ENV IS: {environment}");

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            if (environment == "Development")
            {
                builder.Services.AddSingleton<IBoardingPassRepository, DevBoardingPassRepository>();
                builder.Services.AddSingleton<IBookingRepository, DevBookingRepository>();
                builder.Services.AddSingleton<ICheckinRepository, DevCheckinRepository>();
                builder.Services.AddSingleton<IFlightInfoRepository, DevFlightInfoRepository>();
            } {
                builder.Services.AddScoped<IBoardingPassRepository, ProdBoardingPassRepository>();
                builder.Services.AddScoped<IBookingRepository, ProdBookingRepository>();
                builder.Services.AddScoped<ICheckinRepository, ProdCheckinRepository>();
                builder.Services.AddScoped<IFlightInfoRepository, ProdFlightInfoRepository>();
                builder.Services.AddScoped<IBaggageRepository, ProdBaggageRepository>();
            }
            if (environment == "Production") {
                builder.Services.AddSingleton<RabbitMQConnection>();
                builder.Services.AddSingleton<RabbitMQChannel>();
            }

            builder.Services.AddCors(options => {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5012",
                                "http://localhost",
                                "http://127.0.0.1")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    });
            });

            var app = builder.Build();
            if (app.Environment.IsProduction()) {
                app.UseRabbit();
            } else {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}