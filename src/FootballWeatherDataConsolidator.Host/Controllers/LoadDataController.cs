using FootballWeatherDataConsolidator.Logic.IService;
using Microsoft.AspNetCore.Mvc;

namespace FootballWeatherDataConsolidator.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadDataController : ControllerBase
    {
        private readonly ILoadDataService _service;
        public LoadDataController(ILoadDataService service) {
            _service = service;
        }

        [HttpPut]
        [Route("gameData")]
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

                await _service.LoadGameDataAsync(fileLines);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to process file contents");
            }

            return true;

        }
    }
}
