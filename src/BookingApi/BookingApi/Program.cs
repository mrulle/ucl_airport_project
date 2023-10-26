
using Microsoft.AspNetCore.DataProtection.Repositories;
using BookingApi.Persistance;

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
            if (environment == "Development")
            {
                builder.Services.AddSingleton<IBoardingPassRepository, DevBoardingPassRepository>();
                builder.Services.AddSingleton<IBookingRepository, DevBookingRepository>();
                builder.Services.AddSingleton<ICheckinRepository, DevCheckinRepository>();
                builder.Services.AddSingleton<IFlightInfoRepository, DevFlightInfoRepository>();
            }
                var app = builder.Build();

            // Configure the HTTP request pipeline.
            // NOTE: Add implementation of PROD Repository
            if (app.Environment.IsDevelopment())
            {
                
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}