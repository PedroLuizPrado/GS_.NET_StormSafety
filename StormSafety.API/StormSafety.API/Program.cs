using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StormSafety.API.Data;
using StormSafety.API.Services;
using Swashbuckle.AspNetCore.Annotations;
using AspNetCoreRateLimit;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API SOS Tempestades",
        Version = "v1",
        Description = "Permite registrar ocorrências de desastres climáticos com nome, localização e tipo do problema."
    });
});

// RabbitMQ e ML.NET services
builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddSingleton<MLModelService>(); // 🔍 Aqui o ML

var app = builder.Build();

// Banco
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIpRateLimiting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
