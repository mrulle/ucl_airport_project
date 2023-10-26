using BookingApi.Persistance;
using BookingApi.RabbitMQ;

namespace BookingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
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
            builder.Services.AddSingleton<RabbitMQConnection>();
            builder.Services.AddSingleton<RabbitMQChannel>();
            if (environment == "Development")
            {
                builder.Services.AddSingleton<IBoardingPassRepository, DevBoardingPassRepository>();
                builder.Services.AddSingleton<IBookingRepository, DevBookingRepository>();
                builder.Services.AddSingleton<ICheckinRepository, DevCheckinRepository>();
                builder.Services.AddSingleton<IFlightInfoRepository, DevFlightInfoRepository>();
            } else {
                builder.Services.AddScoped<IBoardingPassRepository, ProdBoardingPassRepository>();
                builder.Services.AddScoped<IBookingRepository, ProdBookingRepository>();
                builder.Services.AddScoped<ICheckinRepository, ProdCheckinRepository>();
                builder.Services.AddScoped<IFlightInfoRepository, ProdFlightInfoRepository>();
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
            // app.UseRabbit();
            // Configure the HTTP request pipeline.
            // NOTE: Add implementation of PROD Repository
            if (app.Environment.IsDevelopment())
            {
                
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