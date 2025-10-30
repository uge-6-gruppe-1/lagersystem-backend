using Backend.Application.Services;
using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Interfaces.Services;
using Backend.Infrastructure.Persistence.Contexts;
using Backend.Infrastructure.Persistence.Repositories;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

// Load environment variables from .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CORS configuration ---
// Allow all origins, methods, and headers (for development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Entity Framework with PostgreSQL
var dbHost = Environment.GetEnvironmentVariable("DB_HOST")
    ?? throw new InvalidOperationException("DB_HOST environment variable is not set.");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT")
    ?? throw new InvalidOperationException("DB_PORT environment variable is not set.");
var dbName = Environment.GetEnvironmentVariable("DB_NAME")
    ?? throw new InvalidOperationException("DB_NAME environment variable is not set.");
var dbUser = Environment.GetEnvironmentVariable("DB_USER")
    ?? throw new InvalidOperationException("DB_USER environment variable is not set.");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD")
    ?? throw new InvalidOperationException("DB_PASSWORD environment variable is not set.");

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILocationService, LocationService>();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- Enable CORS middleware ---
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Configure database retry logic
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    const int maxRetries = 30;
    var retryCount = 0;

    while (retryCount < maxRetries)
    {
        try
        {
            logger.LogInformation("Attempting to connect to database (attempt {RetryCount}/{MaxRetries})", retryCount + 1, maxRetries);
            context.Database.EnsureCreated();
            logger.LogInformation("Successfully connected to database and ensured it exists");
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            if (retryCount >= maxRetries)
            {
                logger.LogError(ex, "Failed to connect to database after {MaxRetries} attempts", maxRetries);
                throw;
            }
            logger.LogWarning(ex, "Failed to connect to database (attempt {RetryCount}/{MaxRetries}). Retrying in 2 seconds...", retryCount, maxRetries);
            await Task.Delay(2000);
        }
    }
}

app.Run();
