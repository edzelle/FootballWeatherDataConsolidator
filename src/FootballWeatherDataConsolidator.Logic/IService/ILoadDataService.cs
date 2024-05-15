using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Logic.IService
{
    public interface ILoadDataService
    {
        Task LoadGameDataAsync(List<string> fileLines);
    }
}
