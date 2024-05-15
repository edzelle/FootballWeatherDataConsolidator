using FootballWeatherDataConsolidator.Data;
using FootballWeatherDataConsolidator.Data.Entites;
using FootballWeatherDataConsolidator.Logic.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Logic.Service
{
    public class GameService : IGameService
    {
        private readonly FootballContext _context;
        private readonly ILogger<IGameService> _logger;
        public GameService(FootballContext footballContext, ILogger<IGameService> logger)
        {
            _context = footballContext;
            _logger = logger;
        }

        public async Task<List<GameEntity>> GetAllGamesAsync()
        {
            return await _context.Games.ToListAsync();
        }

    }
}
