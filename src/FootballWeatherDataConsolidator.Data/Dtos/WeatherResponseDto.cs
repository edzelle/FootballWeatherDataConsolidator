using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Data.Dtos
{
    public class WeatherResponseDto
    {
        public Hourly Hourly { get; set; }
    }

    public class Hourly { 
        public List<DateTime> Time { get; set; }

        public List<decimal> Temperature_2m { get; set; }
        
        public List<decimal> Relative_humidity_2m { get; set; }

        public List<decimal> Apparent_temperature { get; set; }

        public List<decimal> Rain { get; set; }

        public List<decimal> Snowfall { get; set;  }

        public List<decimal> Wind_speed_10m { get; set; }

        public List<decimal> Wind_gusts_10m { get; set; }

    }
}
