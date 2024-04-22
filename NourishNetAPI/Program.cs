
using FoodSharing.Application.Interfaces;
using FoodSharing.Application.Services;
using Microsoft.EntityFrameworkCore;
using NourishNet.Repository.Data;

namespace NourishNetAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IFoodSharingDbContext, FoodSharingDbContext>();

            builder.Services.AddDbContext<FoodSharingDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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