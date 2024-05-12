using Microsoft.EntityFrameworkCore;
using ProductInventoryManagement.Entities;

namespace ProductInventoryManagement.Data
{
    public class ProductInventoryManagementDbContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
        protected readonly IConfiguration Configuration;
        public ProductInventoryManagementDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
