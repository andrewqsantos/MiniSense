using Microsoft.EntityFrameworkCore;
using MiniSense.API.Converters;
using MiniSense.Application.Interfaces;
using MiniSense.Application.Services;
using MiniSense.Domain.Interfaces.Repositories;
using MiniSense.Infrastructure.Persistence;
using MiniSense.Infrastructure.Repositories;
using MiniSense.Infrastructure.Services;
using System.Text.Json.Serialization;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IMiniSenseService, MiniSenseService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISensorDeviceRepository, SensorDeviceRepository>();
builder.Services.AddScoped<IDataStreamRepository, DataStreamRepository>();
builder.Services.AddScoped<IMeasurementUnitRepository, MeasurementUnitRepository>();
builder.Services.AddScoped<IDeviceQueryService, DeviceQueryService>();
builder.Services.AddScoped<ISensorDataRepository, SensorDataRepository>();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
    options.JsonSerializerOptions.Converters.Add(new GuidNoHyphensConverter());
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi("v1", options => 
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "MiniSense API";
        document.Info.Version = "v1";
        document.Info.Description = "API Desafio MiniSense";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try { context.Database.Migrate(); DbInitializer.Seed(context); } catch { }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapOpenApi();

app.MapScalarApiReference(options =>
{
    options
        .WithTitle("MiniSense API Docs")
        .WithTheme(ScalarTheme.Mars)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.Run();