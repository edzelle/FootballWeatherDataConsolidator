using FootballWeatherDataConsolidator.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Logic.IService
{
    public interface IWeatherService
    {
        Task<WeatherAverageDto> ForeastWeatherForGame(string homeTeam, DateTime startTimeAndDate, int gmtOffset);

        Task<WeatherAverageDto> GetWeatherDataAsync(decimal latitude, decimal longitude, DateTime startDate, DateTime startTime, int timeZoneOffset);
        
        Task LoadWeatherDataForGames();
     }
}
