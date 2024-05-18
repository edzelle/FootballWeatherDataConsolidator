using FootballWeatherDataConsolidator.Client;
using FootballWeatherDataConsolidator.Data;
using FootballWeatherDataConsolidator.Data.Dtos;
using FootballWeatherDataConsolidator.Data.Entites;
using FootballWeatherDataConsolidator.Logic.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Logic.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly FootballContext _context;
        private readonly ILogger<IWeatherService> _logger;
        private readonly WeatherHttpClient _client;

        private const string hourlyMetrics = "temperature_2m,relative_humidity_2m,apparent_temperature,rain,snowfall,wind_speed_10m,wind_gusts_10m";

        private const string tempUnits = "fahrenheit";

        private const string speedUnits = "mph";

        private const string precipitationUnit = "inch";

        public WeatherService(FootballContext context,  ILogger<IWeatherService> logger, WeatherHttpClient client)
        {
            _context = context;
            _logger = logger;
            _client = client;
        }

        public async Task LoadWeatherDataForGames()
        {
            var games = await _context.Games.Include(x => x.GameWeatherEntity).Where(x => x.GameWeatherEntity == null).ToListAsync();
            foreach (var game in games)
            {
                await LoadGameData(game);
            }
        }

        private async Task LoadGameData(GameEntity game)
        {
            if (game.HomeTeam.Contains("Washington Football Team"))
            {
                game.HomeTeam = "Washington Commanders";
            }

            var venueId = await _context.Teams.Where(x => x.Name == game.HomeTeam).Include(x => x.TeamPlaysInStadium).Select(x => x.TeamPlaysInStadium.StadiumId).FirstOrDefaultAsync();
            var venue = await _context.Stadiums.FirstOrDefaultAsync(x => x.Id == venueId);
            if (venue != null)
            {
                var weatherDataForGame = await GetWeatherDataAsync(venue.Lattitude, venue.Longitude, game.GameDate, game.GameDate + game.StartTime.ToTimeSpan(), game.GMTOffset);

                var weatherGameEntity = new GameWeatherEntity()
                {
                    GameId = game.Id,
                    AverageApparentTempurature = weatherDataForGame.AverageApparentTempurature,
                    AverageRain = weatherDataForGame.AverageRain,
                    AverageRelativeHumitidty2M = weatherDataForGame.AverageRelativeHumitidty,
                    AverageSnowfall = weatherDataForGame.AverageSnowfall,
                    AverageTempurature2M = weatherDataForGame.AverageTempurature,
                    AverageWindSpeed10M = weatherDataForGame.AverageWindSpeed,
                    AverageWindGusts10M = weatherDataForGame.AverageWindGusts,
                };

                _context.Add(weatherGameEntity);

                await _context.SaveChangesAsync();
            } else
            {
                _logger.LogError("Unable to find venue Id for home team:" + game.HomeTeam);
            }


        }

        public async Task<WeatherAverageDto> GetWeatherDataAsync(decimal lattitude, decimal longitude, DateTime startDate, DateTime startTime, int timeZoneOffset)
        {
            // Get Data for Two Day Range so UTC offset will include all data for hours of game
            var weather = await GetWeatherResponse(lattitude, longitude, startDate, startDate.AddDays(1));

            var weatherAverageDto = ComputeWeatherAverages(weather, timeZoneOffset, startTime);

            return weatherAverageDto;
        }

        public async Task<WeatherAverageDto> ForeastWeatherForGame(string homeTeam, DateTime startTimeAndDate, int gmtOffset) {
            var team = await _context.Teams.Where(x => x.Name.ToLower() == homeTeam.ToLower()).Include(y=> y.TeamPlaysInStadium).FirstOrDefaultAsync();
            if (team  == null)
            {
                throw new Exception("Unable to find team with name: " + homeTeam);
            }
            var venue = await _context.Stadiums.FirstOrDefaultAsync(x => x.Id == team.TeamPlaysInStadium.StadiumId);
            if (venue != null)
            {
                var weatherDataForGame = await GetWeatherForecastDataAsync(venue.Lattitude, venue.Longitude, startTimeAndDate.Date, startTimeAndDate, gmtOffset);
                return weatherDataForGame;
            }
            throw new Exception("Unable to get forcast for game");
        }

        public async Task<WeatherAverageDto> GetWeatherForecastDataAsync(decimal lattitude, decimal longitude, DateTime startDate, DateTime startTime, int timeZoneOffset)
        {
            try
            {
                // Get Data for Two Day Range so UTC offset will include all data for hours of game
                var weather = await GetWeatherForecastResponse(lattitude, longitude, startDate, startDate.AddDays(1));

                var weatherAverageDto = ComputeWeatherAverages(weather, timeZoneOffset, startTime);

                return weatherAverageDto;
            } catch (Exception ex)
            {
                throw new Exception("Forecast data unavailible for entered date. Open Metro API only has forcast data 16 days in he future and 6 months in the past.");
            }
        }

        private WeatherAverageDto ComputeWeatherAverages(WeatherResponseDto weather, int timeZoneOffset, DateTime startTime)
        {
            var utcStartTime = startTime.AddHours(-1 * timeZoneOffset);

            var startIndex = weather.Hourly.Time.IndexOf(weather.Hourly.Time.FirstOrDefault(x => DateTime.Compare(x, utcStartTime) > 0)) -1;

            // Assume Games are three hours in duration

            return new WeatherAverageDto()
            {
                AverageTempurature = weather.Hourly.Temperature_2m.GetRange(startIndex, 4).Average(),
                AverageApparentTempurature = weather.Hourly.Apparent_temperature.GetRange(startIndex, 4).Average(),
                AverageRain = weather.Hourly.Rain.GetRange(startIndex, 4).Average(),
                AverageRelativeHumitidty = weather.Hourly.Relative_humidity_2m.GetRange(startIndex, 4).Average(),
                AverageSnowfall = weather.Hourly.Snowfall.GetRange(startIndex, 4).Average(),
                AverageWindSpeed = weather.Hourly.Wind_speed_10m.GetRange(startIndex, 4).Average(),
                AverageWindGusts = weather.Hourly.Wind_gusts_10m.GetRange(startIndex, 4).Average(),
            };
        }

        private async Task<WeatherResponseDto> GetWeatherResponse(decimal lattitude, decimal longitude,DateTime startDate, DateTime endDate)
        {
            var query = string.Format("v1/archive?latitude={0}&longitude={1}&start_date={2}&end_date={3}&hourly={4}&temperature_unit={5}&wind_speed_unit={6}&precipitation_unit={7}",
                                                                                            lattitude, longitude, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), hourlyMetrics, tempUnits, speedUnits, precipitationUnit);
            HttpResponseMessage response = await _client.GetAsync(query);

            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            return JsonConvert.DeserializeObject<WeatherResponseDto>(jsonResponse, jsonSerializerSettings);
        }

        private async Task<WeatherResponseDto> GetWeatherForecastResponse(decimal lattitude, decimal longitude, DateTime startDate, DateTime endDate)
        {
            var query = string.Format("v1/forecast?latitude={0}&longitude={1}&start_date={2}&end_date={3}&hourly={4}&temperature_unit={5}&wind_speed_unit={6}&precipitation_unit={7}",
                                                                                            lattitude, longitude, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), hourlyMetrics, tempUnits, speedUnits, precipitationUnit);
            HttpResponseMessage response = await _client.GetAsync(query);

            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            return JsonConvert.DeserializeObject<WeatherResponseDto>(jsonResponse, jsonSerializerSettings);
        }
    }
}
