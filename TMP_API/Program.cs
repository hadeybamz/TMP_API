using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TMP_API.Context;
using TMP_API.Repository;
using TMP_API.Repository.IRepository;
using TMP_API.Services;
using TMP_API.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    var services = builder.Services;

    services.AddDbContext<DataContext>();
    services.AddCors();
    services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "TMP WEB API",
            Description = "Interview Technical Assessment Solution",
            Contact = new OpenApiContact
            {
                Name = "Bamgbala Shuaib Adeyemi",
                Email = "adeyemi.bamgbala@gmail.com",
                Url = new Uri("https://github.com/hadeybamz")
            }
        });
    });

    services.AddTransient<IProductRepository, ProductRepository>();
    services.AddTransient<IOrderRepository, OrderRepository>();
    services.AddTransient<ICustomerRepository, CustomerRepository>();

    services.AddTransient<IProductService, ProductService>();
    services.AddTransient<IOrderService, OrderService>();
    services.AddTransient<ICustomerService, CustomerService>();

}

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "TMP API"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
