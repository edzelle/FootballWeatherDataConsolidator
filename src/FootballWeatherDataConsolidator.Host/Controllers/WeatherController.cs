using FootballWeatherDataConsolidator.Data.Dtos;
using FootballWeatherDataConsolidator.Data.Entites;
using FootballWeatherDataConsolidator.Logic.IService;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<bool>> LoadWeatherDataForGames()
        {
            await _service.LoadWeatherDataForGames();
            return true;
        }

        [HttpGet]
        public async Task<ActionResult<WeatherResponseDto>> GetWeatherDataAsync()
        {
            //return await _service.GetWeatherDataAsync();
            var x = await Task.FromResult(true);
            return null;
        }
    }
}
