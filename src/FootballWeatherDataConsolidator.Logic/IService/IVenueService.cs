﻿using FootballWeatherDataConsolidator.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Logic.IService
{
    public interface IVenueService
    {
        Task<List<VenueEntity>> GetAllVenuesAsync();
        Task<List<TeamEntity>> GetTeamsForVenueId(int venueId);
    }
}
