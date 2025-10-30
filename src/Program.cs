using Backend.Application.Services;
using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Interfaces.Services;
using Backend.Infrastructure.Persistence.Contexts;
using Backend.Infrastructure.Persistence.Repositories;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Backend.Domain.Entities;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.IO;

// Load environment variables from .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Configure database retry logic and seeding
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

      // Seed products from CSV only if table is empty
      if (!await context.Product.AnyAsync())
      {
        var seedPath = Path.Combine(app.Environment.ContentRootPath, "Seed", "products.csv");
        if (File.Exists(seedPath))
        {
          var lines = await File.ReadAllLinesAsync(seedPath);
          if (lines.Length > 0)
          {
            var seeded = 0;
            // If first line is a header, skip it (simple heuristic: contains "name" or "price")
            var startIndex = 0;
            if (lines[0].ToLowerInvariant().Contains("name") || lines[0].ToLowerInvariant().Contains("price"))
              startIndex = 1;

            for (int i = startIndex; i < lines.Length; i++)
            {
              var line = lines[i].Trim();
              if (string.IsNullOrWhiteSpace(line)) continue;

              var fields = SplitCsvLine(line);
              // Expect: Name, Description, Price, [ImagePath]
              if (fields.Length < 3)
              {
                logger.LogWarning("Skipping CSV line {LineNumber}: not enough columns", i + 1);
                continue;
              }

              var name = fields[0];
              var description = fields[1];
              if (!decimal.TryParse(fields[2], NumberStyles.Number | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var price))
              {
                logger.LogWarning("Skipping CSV line {LineNumber}: invalid price '{PriceField}'", i + 1, fields[2]);
                continue;
              }

              var imagePath = fields.Length > 3 ? fields[3] : string.Empty;

              var product = new Product
              {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Price = price,
                ImagePath = imagePath ?? string.Empty
              };

              context.Product.Add(product);
              seeded++;
            }

            if (seeded > 0)
            {
              await context.SaveChangesAsync();
              logger.LogInformation("Seeded {SeededCount} products from {SeedPath}", seeded, seedPath);
            }
            else
            {
              logger.LogInformation("No valid product rows found in {SeedPath}", seedPath);
            }
          }
        }
        else
        {
          logger.LogWarning("Product seed file not found at {SeedPath}. Skipping seeding.", Path.Combine(app.Environment.ContentRootPath, "Seed", "products.csv"));
        }
      }
      else
      {
        logger.LogInformation("Product table is not empty. Skipping CSV seeding.");
      }

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

// Local helper to split CSV lines respecting quoted fields
static string[] SplitCsvLine(string line)
{
  var values = new List<string>();
  var sb = new StringBuilder();
  var inQuotes = false;

  for (int i = 0; i < line.Length; i++)
  {
    var ch = line[i];
    if (ch == '"')
    {
      // handle escaped quotes
      if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
      {
        sb.Append('"');
        i++; // skip next quote
      }
      else
      {
        inQuotes = !inQuotes;
      }
    }
    else if (ch == ',' && !inQuotes)
    {
      values.Add(sb.ToString().Trim());
      sb.Clear();
    }
    else
    {
      sb.Append(ch);
    }
  }

  values.Add(sb.ToString().Trim());
  // Trim surrounding quotes from values
  for (int i = 0; i < values.Count; i++)
  {
    var v = values[i];
    if (v.Length >= 2 && v.StartsWith("\"") && v.EndsWith("\""))
      v = v.Substring(1, v.Length - 2).Replace("\"\"", "\"");
    values[i] = v;
  }

  return values.ToArray();
}