using FootballWeatherDataConsolidator.Data;
using FootballWeatherDataConsolidator.Data.Entites;
using FootballWeatherDataConsolidator.Logic.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Logic.Service
{
    public class LoadDataService: ILoadDataService
    {
        private readonly FootballContext _context;
        private readonly ILogger<ILoadDataService> _logger;
        public LoadDataService(FootballContext footballContext, ILogger<ILoadDataService> logger) { 
            _context = footballContext;
            _logger = logger;
        }
        public async Task LoadGameDataAsync(List<string> fileLines)
        {
            int rowNum = 0;
            foreach(var line in fileLines)
            {
                try
                {
                    var fields = line.Split(',');

                    var gameEntity = new GameEntity()
                    {
                        Season = int.Parse(fields[0]),
                        Week = int.Parse(fields[1]),
                        GameDate = DateTime.Parse(fields[2]),
                        StartTime = TimeOnly.Parse(fields[3]),
                        GMTOffset = int.Parse(fields[4]),
                        GameSite = fields[5],
                        HomeTeam = fields[6],
                        HomeTeamScore = int.Parse(fields[7]),
                        AwayTeam = fields[8],
                        AwayTeamScore = int.Parse(fields[9])
                    };

                    // check if record already exits

                    if (!await _context.GameEntities.AnyAsync(x => x.Season == gameEntity.Season
                                                      && x.Week == gameEntity.Week
                                                      && x.HomeTeam == gameEntity.HomeTeam))
                    {
                        _context.Add(gameEntity);
                        rowNum++;
                    }
                } catch (Exception ex)
                {
                    _logger.LogError("Unable to parse data on row: " + rowNum.ToString());
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception("Unable to save Game Entites");
            }
        }

    }
}
