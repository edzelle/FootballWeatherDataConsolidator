using FootballWeatherDataConsolidator.Data.Entites;
using FootballWeatherDataConsolidator.Logic.IService;
using Microsoft.AspNetCore.Mvc;

namespace FootballWeatherDataConsolidator.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILoadDataService _loadService;
        private readonly IGameService _gameService;
        public GameController(ILoadDataService loadService, IGameService gameService) {
            _loadService = loadService;
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameEntity>>> GetGames()
        {
            return await _gameService.GetAllGamesAsync();
        }



        [HttpPut]
        [Route("load")]
        public async Task<ActionResult<bool>> LoadGameDataFromFileAsync(IFormFile gamesFile)
        {
            if (gamesFile == null) {
                return BadRequest("No file submitted");
            }

            if (!gamesFile.FileName.EndsWith(".csv"))
            {
                return BadRequest("Only csv files may be uploaded");
            }

            try
            {
                var streamReader = new StreamReader(gamesFile.OpenReadStream());
                var fileContent = streamReader.ReadToEnd().Trim();
                List<string> fileLines = fileContent.Split("\r\n").Skip(1).ToList();

                await _loadService.LoadGameDataAsync(fileLines);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to process file contents");
            }

            return true;

        }
    }
}
