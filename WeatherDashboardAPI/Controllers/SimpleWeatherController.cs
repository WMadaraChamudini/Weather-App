using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WeatherDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimpleWeatherController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey = "33439aabeffd21964c4f17a38eda96e0";
        private readonly string _baseUrl = "https://api.openweathermap.org/data/2.5";

        public SimpleWeatherController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Weather API is working!", timestamp = DateTime.Now });
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("City name is required");
            }

            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var url = $"{_baseUrl}/weather?q={city}&appid={_apiKey}&units=metric";
                
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound($"City '{city}' not found");
                    }
                    return BadRequest("Error fetching weather data");
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                
                // Parse the OpenWeatherMap response
                using var doc = JsonDocument.Parse(jsonString);
                var root = doc.RootElement;

                var result = new
                {
                    city = root.GetProperty("name").GetString(),
                    temperature = Math.Round(root.GetProperty("main").GetProperty("temp").GetDouble(), 1),
                    condition = root.GetProperty("weather")[0].GetProperty("main").GetString(),
                    description = root.GetProperty("weather")[0].GetProperty("description").GetString(),
                    iconUrl = $"https://openweathermap.org/img/wn/{root.GetProperty("weather")[0].GetProperty("icon").GetString()}@2x.png"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetForecast([FromQuery] string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("City name is required");
            }

            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var url = $"{_baseUrl}/forecast?q={city}&appid={_apiKey}&units=metric";
                
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound($"City '{city}' not found");
                    }
                    return BadRequest("Error fetching forecast data");
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                
                // Parse the OpenWeatherMap response
                using var doc = JsonDocument.Parse(jsonString);
                var root = doc.RootElement;

                var forecasts = new List<object>();
                var list = root.GetProperty("list");
                
                // Get one forecast per day (every 8th item = 24 hours / 3 hours)
                for (int i = 0; i < Math.Min(list.GetArrayLength(), 40); i += 8)
                {
                    if (forecasts.Count >= 5) break;
                    
                    var item = list[i];
                    forecasts.Add(new
                    {
                        date = DateTimeOffset.FromUnixTimeSeconds(item.GetProperty("dt").GetInt64()).DateTime,
                        temperature = Math.Round(item.GetProperty("main").GetProperty("temp").GetDouble(), 1),
                        condition = item.GetProperty("weather")[0].GetProperty("main").GetString(),
                        description = item.GetProperty("weather")[0].GetProperty("description").GetString(),
                        iconUrl = $"https://openweathermap.org/img/wn/{item.GetProperty("weather")[0].GetProperty("icon").GetString()}@2x.png"
                    });
                }

                var result = new
                {
                    city = root.GetProperty("city").GetProperty("name").GetString(),
                    forecasts = forecasts
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}