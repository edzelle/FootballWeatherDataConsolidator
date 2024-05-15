using FootballWeatherDataConsolidator.Data.Entites;
using FootballWeatherDataConsolidator.Data;
using FootballWeatherDataConsolidator.Logic.IService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FootballWeatherDataConsolidator.Logic.Service
{
    public class VenueService : IVenueService
    {
        private readonly FootballContext _context;
        private readonly ILogger<IGameService> _logger;
        public VenueService(FootballContext footballContext, ILogger<IGameService> logger)
        {
            _context = footballContext;
            _logger = logger;
        }

        public async Task<List<VenueEntity>> GetAllVenuesAsync()
        {
            return await _context.Stadiums.ToListAsync();
        }

        public async Task<List<TeamEntity>> GetTeamsForVenueId(int venueId)
        {
            var teams = await _context.TeamPlaysInStadium.Where(x => x.StadiumId == venueId).Select(x => x.TeamId).ToListAsync();
            return await _context.Teams.Where(x => teams.Contains(x.Id)).ToListAsync();
        }
    }
}
