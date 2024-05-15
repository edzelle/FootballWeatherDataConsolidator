using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Data.Entites
{
    public class GameEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Season { get; set; }

        public int Week { get; set; }

        public DateTime GameDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public int GMTOffset { get; set; }

        public string? GameSite { get; set; }

        public string? HomeTeam { get; set; }

        public int HomeTeamScore { get; set; }

        public string? AwayTeam { get; set; }

        public int AwayTeamScore { get; set; }
    }
}
