using Microsoft.EntityFrameworkCore;
using Serilog;
using SustainableMaterialsRecommender.Application.Interfaces;
using SustainableMaterialsRecommender.Application.Services;
using SustainableMaterialsRecommender.Core.Interfaces;
using SustainableMaterialsRecommender.Infrastructure.Data;
using SustainableMaterialsRecommender.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

Log.Information("Starting web application");

// Configure services
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
ConfigureMiddleware(app, app.Environment);

// Configure endpoints
ConfigureEndpoints(app);

Log.Information("Application configured. Starting to run.");

try
{
    app.Run();
    Log.Information("Application stopped cleanly.");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    Log.Information("Configuring services");

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        Log.Information("Configured database context with connection string: {ConnectionString}",
            configuration.GetConnectionString("DefaultConnection"));
    });

    services.AddScoped<IMaterialRepository, MaterialRepository>();
    services.AddScoped<IMaterialService, MaterialService>();
    services.AddSingleton(TimeProvider.System);
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.WriteIndented = true;
        options.SerializerOptions.IncludeFields = true;
    });

    Log.Information("Services configured successfully");
}

void ConfigureMiddleware(WebApplication app, IWebHostEnvironment env)
{
    Log.Information("Configuring middleware");

    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        Log.Information("Development environment: Swagger enabled");
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthorization();

    Log.Information("Middleware configured successfully");
}

void ConfigureEndpoints(WebApplication app)
{
    Log.Information("Configuring endpoints");

    app.MapControllers();

    app.MapGet("/api/materials/stats", async (IMaterialService materialService, ILogger<Program> logger) =>
    {
        logger.LogInformation("Retrieving material statistics");
        var materials = await materialService.GetAllMaterialsAsync();
        var stats = new
        {
            TotalMaterials = materials.Count(),
            AverageSustainabilityScore = materials.Average(m => m.SustainabilityScore),
            TopCategories = materials.GroupBy(m => m.Category)
                                     .OrderByDescending(g => g.Count())
                                     .Take(3)
                                     .Select(g => new { Category = g.Key, Count = g.Count() })
        };
        logger.LogInformation("Material statistics retrieved: {@Stats}", stats);
        return Results.Ok(stats);
    });

    app.MapGet("/api/healthcheck", (ILogger<Program> logger) =>
    {
        logger.LogInformation("Health check endpoint hit");
        return Results.Ok(new { status = "Healthy", message = "API is running" });
    });

    app.MapFallback(context =>
    {
        Log.Warning("Fallback route hit for {Path}", context.Request.Path);
        return Task.FromResult(Results.NotFound($"The requested resource was not found: {context.Request.Path}"));
    });

    Log.Information("Endpoints configured successfully");
}