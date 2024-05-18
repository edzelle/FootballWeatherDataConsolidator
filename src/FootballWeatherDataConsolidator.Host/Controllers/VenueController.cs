using FootballWeatherDataConsolidator.Data.Entites;
using FootballWeatherDataConsolidator.Logic.IService;
using FootballWeatherDataConsolidator.Logic.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FootballWeatherDataConsolidator.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VenueController : ControllerBase
    {
        private readonly ILoadDataService _service;
        private readonly IVenueService _venueService;
        public VenueController(ILoadDataService service, IVenueService venueService) {
            _service = service;
            _venueService = venueService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Returns list of all venues from DB")]

        public async Task<ActionResult<List<VenueEntity>>> GetVenues()
        {
            return await _venueService.GetAllVenuesAsync();
        }


        [HttpGet]
        [Route("teams")]
        [SwaggerOperation(Summary = "Returns list of all teams from DB")]

        public async Task<ActionResult<List<TeamEntity>>> GetTeamsForVenueId(int venueId)
        {
            return await _venueService.GetTeamsForVenueId(venueId);
        }

        [HttpPut]
        [Route("load")]
        [SwaggerOperation(Summary = "Loads games from csv file and saves to DB")]

        public async Task<ActionResult<bool>> LoadVenueDataFromFileAsync(IFormFile gamesFile)
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

                await _service.LoadVenueDataAsync(fileLines);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to process file contents");
            }

            return true;

        }
    }
}
