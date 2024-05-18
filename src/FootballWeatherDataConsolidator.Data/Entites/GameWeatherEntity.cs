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

        public decimal AverageTempurature2M { get; set; }

        public decimal AverageRelativeHumitidty2M { get; set; }

        public decimal AverageApparentTempurature { get; set; }

        public decimal AverageRain { get; set; }

        public decimal AverageSnowfall { get; set; }

        public decimal AverageWindSpeed10M { get; set; }

        public decimal AverageWindGusts10M { get; set; }

        public virtual GameEntity Game { get; set; }

    }
}
