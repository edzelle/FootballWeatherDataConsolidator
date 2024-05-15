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
using System.Text.RegularExpressions;
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
                    if(fields.Length != 10)
                    {
                        throw new Exception("Length of line does not match expected length");
                    }
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

                    if (!await _context.Games.AnyAsync(x => x.Season == gameEntity.Season
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

        public async Task LoadVenueDataAsync(List<string> fileLines)
        {
            int rowNum = 0;
            foreach (var line in fileLines)
            {
                try
                {
                    var fields = Regex.Split(line, "[,]{1}(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                    if (fields.Length != 8)
                    {
                        throw new Exception("Length of line does not match expected length");
                    }
                    var venueEntity = new VenueEntity()
                    {
                        Name = fields[0],
                        Capacity = int.Parse(fields[1]),
                        LocationCity = fields[2].Replace("\"", "").Split(',')[0],
                        LocationState = fields[2].Replace("\"","").Split(',')[1],
                        Surface = fields[3],
                        RoofType = fields[4],
                        Opened = int.Parse(fields[6]),
                        Lattitude = decimal.Parse(fields[7].Replace("\"", "").Split(",")[0]),
                        Longitude = decimal.Parse(fields[7].Replace("\"", "").Split(",")[1].Substring(1)),
                    };

                    // check if record already exits

                    if (!await _context.Stadiums.AnyAsync(x => x.Name == venueEntity.Name
                                                      && x.Opened == venueEntity.Opened))
                    {
                        _context.Add(venueEntity);
                        await _context.SaveChangesAsync();
                    }

                    foreach(string team in fields[5].Split(","))
                    {
                        if (!await _context.Teams.AnyAsync(x => x.Name == team))
                        {
                            var teamEntity = new TeamEntity()
                            {
                                Name = team

                            };
                            _context.Add(teamEntity);
                            await _context.SaveChangesAsync();

                            var tpis = new TeamPlaysInStadiumEntity()
                            {
                                StadiumId = venueEntity.Id,
                                TeamId = teamEntity.Id,
                            };

                            _context.Add(tpis);
                            await _context.SaveChangesAsync();

                        }
                    }

                    rowNum++;

                }
                catch (Exception ex)
                {
                    _logger.LogError("Unable to parse data on row: " + rowNum.ToString());
                }
            }
        }
    }
}
