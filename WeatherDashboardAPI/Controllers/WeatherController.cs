using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WeatherDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey = "33439aabeffd21964c4f17a38eda96e0";
        private readonly string _baseUrl = "https://api.openweathermap.org/data/2.5";

        public WeatherController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Weather API is working!", timestamp = DateTime.Now });
        }

        [HttpGet("air-quality")]
        public async Task<IActionResult> GetAirQuality([FromQuery] string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("City name is required");
            }

            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                
                // First get coordinates for the city
                var geoUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={_apiKey}";
                var geoResponse = await httpClient.GetAsync(geoUrl);
                
                if (!geoResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Failed to get coordinates for city");
                }

                var geoJsonString = await geoResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var geoData = JsonSerializer.Deserialize<GeoLocationResponse[]>(geoJsonString, options);

                if (geoData == null || geoData.Length == 0)
                {
                    return NotFound($"City '{city}' not found");
                }

                var lat = geoData[0].Lat;
                var lon = geoData[0].Lon;

                // Get air quality data
                var airUrl = $"http://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={_apiKey}";
                var airResponse = await httpClient.GetAsync(airUrl);

                if (!airResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Failed to fetch air quality data");
                }

                var airJsonString = await airResponse.Content.ReadAsStringAsync();
                var airData = JsonSerializer.Deserialize<AirQualityResponse>(airJsonString, options);

                if (airData == null || airData.List == null || airData.List.Length == 0)
                {
                    return BadRequest("Invalid air quality data received");
                }

                var result = new AirQualityResult
                {
                    City = city,
                    AQI = airData.List[0].Main.AQI,
                    Description = GetAQIDescription(airData.List[0].Main.AQI),
                    Components = new AirQualityComponents
                    {
                        CO = airData.List[0].Components.CO,
                        NO2 = airData.List[0].Components.NO2,
                        O3 = airData.List[0].Components.O3,
                        PM2_5 = airData.List[0].Components.PM2_5,
                        PM10 = airData.List[0].Components.PM10
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("historical")]
        public async Task<IActionResult> GetHistoricalWeather([FromQuery] string city, [FromQuery] int days = 5)
        {
            // Mock implementation - in real scenario, you'd call historical weather API
            var random = new Random();
            var historicalData = new List<HistoricalWeatherData>();

            for (int i = 1; i <= days; i++)
            {
                historicalData.Add(new HistoricalWeatherData
                {
                    Date = DateTime.Now.AddDays(-i),
                    Temperature = Math.Round(random.NextDouble() * 30 + 5, 1),
                    Humidity = random.Next(30, 90),
                    Pressure = random.Next(980, 1030)
                });
            }

            return Ok(new { city, historicalData });
        }

        private static string GetAQIDescription(int aqi)
        {
            return aqi switch
            {
                1 => "Good",
                2 => "Fair",
                3 => "Moderate",
                4 => "Poor",
                5 => "Very Poor",
                _ => "Unknown"
            };
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
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var weatherData = JsonSerializer.Deserialize<OpenWeatherMapResponse>(jsonString, options);

                if (weatherData == null || weatherData.Weather == null || weatherData.Weather.Length == 0)
                {
                    return BadRequest("Invalid weather data received");
                }

                var result = new WeatherResponse
                {
                    City = weatherData.Name,
                    Temperature = Math.Round(weatherData.Main.Temp, 1),
                    Condition = weatherData.Weather[0].Main,
                    Description = weatherData.Weather[0].Description,
                    IconUrl = $"https://openweathermap.org/img/wn/{weatherData.Weather[0].Icon}@2x.png"
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
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var forecastData = JsonSerializer.Deserialize<OpenWeatherMapForecastResponse>(jsonString, options);

                if (forecastData == null || forecastData.List == null)
                {
                    return BadRequest("Invalid forecast data received");
                }

                var result = new ForecastResponse
                {
                    City = forecastData.City.Name,
                    Forecasts = forecastData.List
                        .Where((x, i) => i % 8 == 0) // Get one forecast per day (every 8th item = 24 hours / 3 hours)
                        .Take(5)
                        .Select(item => new DailyForecast
                        {
                            Date = DateTimeOffset.FromUnixTimeSeconds(item.Dt).DateTime,
                            Temperature = Math.Round(item.Main.Temp, 1),
                            Condition = item.Weather[0].Main,
                            Description = item.Weather[0].Description,
                            IconUrl = $"https://openweathermap.org/img/wn/{item.Weather[0].Icon}@2x.png"
                        }).ToList()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    // Response models
    public class WeatherResponse
    {
        public string City { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }

    public class ForecastResponse
    {
        public string City { get; set; } = string.Empty;
        public List<DailyForecast> Forecasts { get; set; } = new();
    }

    public class DailyForecast
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }

    // OpenWeatherMap API response models
    public class OpenWeatherMapResponse
    {
        public string Name { get; set; } = string.Empty;
        public MainData Main { get; set; } = new();
        public WeatherInfo[] Weather { get; set; } = Array.Empty<WeatherInfo>();
    }

    public class OpenWeatherMapForecastResponse
    {
        public CityInfo City { get; set; } = new();
        public ForecastItem[] List { get; set; } = Array.Empty<ForecastItem>();
    }

    public class CityInfo
    {
        public string Name { get; set; } = string.Empty;
    }

    public class ForecastItem
    {
        public long Dt { get; set; }
        public MainData Main { get; set; } = new();
        public WeatherInfo[] Weather { get; set; } = Array.Empty<WeatherInfo>();
    }

    public class MainData
    {
        public double Temp { get; set; }
    }

    public class WeatherInfo
    {
        public string Main { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    // Air Quality models
    public class AirQualityResult
    {
        public string City { get; set; } = string.Empty;
        public int AQI { get; set; }
        public string Description { get; set; } = string.Empty;
        public AirQualityComponents Components { get; set; } = new();
    }

    public class AirQualityComponents
    {
        public double CO { get; set; }
        public double NO2 { get; set; }
        public double O3 { get; set; }
        public double PM2_5 { get; set; }
        public double PM10 { get; set; }
    }

    public class AirQualityResponse
    {
        public AirQualityListItem[] List { get; set; } = Array.Empty<AirQualityListItem>();
    }

    public class AirQualityListItem
    {
        public AirQualityMain Main { get; set; } = new();
        public AirQualityComponentsRaw Components { get; set; } = new();
    }

    public class AirQualityMain
    {
        public int AQI { get; set; }
    }

    public class AirQualityComponentsRaw
    {
        public double CO { get; set; }
        public double NO2 { get; set; }
        public double O3 { get; set; }
        public double PM2_5 { get; set; }
        public double PM10 { get; set; }
    }

    // Geolocation models
    public class GeoLocationResponse
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    // Historical weather models
    public class HistoricalWeatherData
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
    }
}