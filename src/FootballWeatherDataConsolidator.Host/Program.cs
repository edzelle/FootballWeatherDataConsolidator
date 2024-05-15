using FootballWeatherDataConsolidator.Data;
using FootballWeatherDataConsolidator.Logic.IService;
using FootballWeatherDataConsolidator.Logic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using NLog.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

var env = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(env);

builder.Services.AddDbContext<FootballContext>(options
         => options.UseSqlite($"Data Source={Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "football.db")}"));


builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();
builder.Services.AddScoped<ILoadDataService, LoadDataService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
