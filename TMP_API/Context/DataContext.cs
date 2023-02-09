using Microsoft.EntityFrameworkCore;
using TMP_API.Models.Customers;
using TMP_API.Models.Orders;
using TMP_API.Models.Products;

namespace TMP_API.Context
{
    public class DataContext : DbContext
    {
        public DataContext(){}

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        private readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to SqlServer database
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
