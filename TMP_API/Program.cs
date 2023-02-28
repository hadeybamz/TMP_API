using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using System.Text;
using TMP_API.Data;
using TMP_API.Helpers;
using TMP_API.Models.Users;
using TMP_API.Repository;
using TMP_API.Repository.IRepository;
using TMP_API.Services;
using TMP_API.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Debug()
.WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();

// Add services to the container.
{
    var services = builder.Services;
    var env = builder.Environment;


    builder.Logging.AddSerilog();

    services.AddCors();
    services.AddControllers();

    services.AddDbContext<DataContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

    services.Configure<IdentityOptions>(options =>
    {
        options.User.RequireUniqueEmail = true;
    });

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
            ValidAudience = builder.Configuration["Jwt:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
            ClockSkew = TimeSpan.Zero, // Messes with expiry!
        };
    });

    services.AddTransient<IClaimsService, ClaimsService>();
    services.AddTransient<IJwtTokenService, JwtTokenService>();

    //services.AddDistributedMemoryCache();
    services.AddMemoryCache();
    services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
    services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
    services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
    services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    services.AddInMemoryRateLimiting();

    var redisOptions = ConfigurationOptions.Parse(builder.Configuration["ConnectionStrings:Redis"]);
    services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(redisOptions));
    services.AddRedisRateLimiting();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "TMP WEB API",
            Description = "Interview Technical Assessment Solution.",
            Contact = new OpenApiContact
            {
                Name = "Bamgbala Shuaib Adeyemi",
                Email = "adeyemi.bamgbala@gmail.com",
                Url = new Uri("https://github.com/hadeybamz/TMP_API")
            }
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
            Name = "Authorization",
            //In = "header",
            //Type = "apiKey",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

    });

    services.AddTransient<IProductRepository, ProductRepository>();
    services.AddTransient<IOrderRepository, OrderRepository>();
    services.AddTransient<IOrderItemRepository, OrderItemRepository>();

    services.AddTransient<IProductService, ProductService>();
    services.AddTransient<IUserService, UserService>();
    services.AddTransient<IOrderService, OrderService>();
    services.AddTransient<IOrderItemService, OrderItemService>();

}

var app = builder.Build();

{
    app.UseIpRateLimiting();
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "TMP API"));
    }

    app.UseCors(x => x
            .SetIsOriginAllowed(origin => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}

app.Run("http://localhost:4000");
