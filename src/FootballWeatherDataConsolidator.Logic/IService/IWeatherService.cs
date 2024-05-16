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
        Task<WeatherAverageDto> GetWeatherDataAsync(decimal lattitude, decimal longitude, DateTime startDate, DateTime startTime, int timeZoneOffset);
        
        Task LoadWeatherDataForGames();
     }
}
