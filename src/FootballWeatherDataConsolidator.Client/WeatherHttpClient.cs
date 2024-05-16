using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FootballWeatherDataConsolidator.Client
{
    public class WeatherHttpClient
    {
        private readonly HttpClient _client;

        public WeatherHttpClient(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("Weather_Client");
        }

        public async Task<HttpResponseMessage> GetAsync(string route)
        {
            try
            {
                return await _client.GetAsync(route).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
