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

string projectDirectory = Path.Join(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName,"database");

builder.Services.AddDbContext<FootballContext>(options
         => options.UseSqlite($"Data Source={Path.Join(projectDirectory, "football.db")}"));


builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();
builder.Services.AddScoped<ILoadDataService, LoadDataService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IVenueService, VenueService>();
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
