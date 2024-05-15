using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Data.Entites
{
    public class TeamPlaysInStadiumEntity
    {
        public int TeamId { get; set; }

        public int StadiumId { get; set; }

        public virtual TeamEntity Team { get; set; }

        public virtual VenueEntity Stadium { get; set; }

    }
}
