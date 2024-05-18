using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Data.Entites
{
    public class VenueEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Capacity { get; set; }

        public string? LocationCity { get; set; }

        public string? LocationState { get; set; }

        public string? Surface { get; set; }

        public string? RoofType { get; set; }

        public int Opened { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public virtual List<TeamPlaysInStadiumEntity> TeamPlaysInStadiumEntites { get; set; }


    }
}
