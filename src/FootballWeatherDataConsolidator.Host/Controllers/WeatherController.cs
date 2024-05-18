using FootballWeatherDataConsolidator.Data.Dtos;
using FootballWeatherDataConsolidator.Data.Entites;
using FootballWeatherDataConsolidator.Logic.IService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FootballWeatherDataConsolidator.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _service;
        public WeatherController(IWeatherService service) {
            _service = service;
        }

        [HttpPut]
        [Route("load")]
        [SwaggerOperation(Summary = "Fetches and saves weather data from open-meteo.com for all games in database that do not have weather data")]

        public async Task<ActionResult<bool>> LoadWeatherDataForGames()
        {
            await _service.LoadWeatherDataForGames();
            return true;
        }

        [HttpGet]
        [Route("forecast")]
        [SwaggerOperation(Summary = "Fetches 3 hour average weather forecast data from open-meteo.com for home stadium of entered team, starting at the time of day entered")]

        public async Task<ActionResult<WeatherAverageDto>> ForeastWeatherForGame(string homeTeam, DateTime startTimeAndDate, int gmtOffset)
        {
           return await _service.ForeastWeatherForGame(homeTeam, startTimeAndDate, gmtOffset);
        }
    }
}
