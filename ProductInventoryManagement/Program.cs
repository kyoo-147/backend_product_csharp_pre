using ProductInventoryManagement.Data;
using ProductInventoryManagement.Filter;
using ProductInventoryManagement.Repositories;
using Serilog;
using System;

namespace ProductInventoryManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            // Thêm dịch vụ vào vùng chứa.

            builder.Services.AddControllers();
            // Tìm hiểu thêm về cách định cấu hình Swagger/OpenAPI tại https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IProductInventoryManagementRepository, ProductInventoryManagementRepository>();
            builder.Services.AddDbContext<ProductInventoryManagementDbContext>();
            
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ProductInventoryManagementExceptionFilter());
            });
            builder.Services.AddSwaggerGen(c =>
            {
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "ProductInventoryManagement.xml");
                c.IncludeXmlComments(filePath);
            });

            var app = builder.Build();

            // Định cấu hình đường dẫn yêu cầu HTTP.
            if (app.Environment.IsDevelopment())
            {
              
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            logger.Information("Ứng dụng đã bắt đầu thành công.");
        }
    }
}
