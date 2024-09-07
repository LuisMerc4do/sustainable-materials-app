using Microsoft.EntityFrameworkCore;
using Serilog;
using SustainableMaterialsRecommender.Application.Interfaces;
using SustainableMaterialsRecommender.Application.Mappings;
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// New .NET 8 feature: Add JSON options globally
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.IncludeFields = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); // Add this line to log HTTP requests

app.UseHttpsRedirection();
app.UseAuthorization();
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

try
{
    Log.Information("Starting web host");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}