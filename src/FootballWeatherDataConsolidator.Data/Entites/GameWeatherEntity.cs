using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Data.Entites
{
    public class GameWeatherEntity
    {
        public int GameId { get; set; }

        public decimal AverageTempurature { get; set; }

        public decimal AverageRelativeHumitidty { get; set; }

        public decimal AverageApparentTempurature { get; set; }

        public decimal AverageRain { get; set; }

        public decimal AverageSnowfall { get; set; }

        public decimal AverageWindSpeed { get; set; }

        public virtual GameEntity Game { get; set; }

    }
}
