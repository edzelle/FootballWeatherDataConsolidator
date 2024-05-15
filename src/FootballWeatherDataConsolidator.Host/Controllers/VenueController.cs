using FootballWeatherDataConsolidator.Data.Entites;
using FootballWeatherDataConsolidator.Logic.IService;
using FootballWeatherDataConsolidator.Logic.Service;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<List<VenueEntity>>> GetVenues()
        {
            return await _venueService.GetAllVenuesAsync();
        }


        [HttpGet]
        [Route("teams")]
        public async Task<ActionResult<List<TeamEntity>>> GetTeamsForVenueId(int venueId)
        {
            return await _venueService.GetTeamsForVenueId(venueId);
        }

        [HttpPut]
        [Route("load")]
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
