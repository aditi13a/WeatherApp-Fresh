using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using WeatherApp.Server.Services;

namespace WeatherApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherHistoryService _weatherHistoryService;

        public WeatherController(IHttpClientFactory httpClientFactory, WeatherHistoryService weatherHistoryService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _weatherHistoryService = weatherHistoryService;
        }

        [HttpGet]
public async Task<IActionResult> GetWeather([FromQuery] string city, [FromQuery] string units = "metric")
{
    if (string.IsNullOrWhiteSpace(city))
        return BadRequest("City parameter is required.");

    try
    {
        var apiKey = "a08476b77942e9e11c55fef3afa4fc5c";
        string url;

        // No country code enforced. Allow full city/country or zip input (like 'london', 'london,uk', or '560001,in')
        if (int.TryParse(city, out _))
        {
            url = $"https://api.openweathermap.org/data/2.5/weather?zip={city}&appid={apiKey}&units={units}";
        }
        else
        {
            url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units={units}";
        }

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, $"API error: {errorContent}");
        }

        var weatherData = await response.Content.ReadFromJsonAsync<WeatherResponse>();
        if (weatherData == null)
        {
            return NotFound("Weather data not found.");
        }

        return Ok(weatherData);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception in GetWeather: {ex}");
        return StatusCode(500, $"Error fetching weather: {ex.Message}");
    }
}

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var history = await _weatherHistoryService.GetAsync();
            return Ok(history);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveWeather([FromBody] WeatherHistory history)
        {
            await _weatherHistoryService.CreateAsync(history);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteWeather(string id)
        {
            await _weatherHistoryService.DeleteAsync(id);
            return Ok();
        }
    }

    // Updated model classes
    public class WeatherResponse
    {
        public MainData Main { get; set; } = new MainData();
        public string Name { get; set; } = string.Empty;
        public List<WeatherInfo> Weather { get; set; } = new();
        public WindData Wind { get; set; } = new WindData();
        public Sys Sys { get; set; } = new Sys();
        public long Dt { get; set; }
    }

    public class WeatherInfo
    {
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    public class MainData
    {
        public double Temp { get; set; }
        public double Feels_like { get; set; }
        public int Humidity { get; set; }
    }

    public class WindData
    {
        public double Speed { get; set; }
    }

    public class Sys
    {
        public long Sunrise { get; set; }
        public long Sunset { get; set; }    // Unix timestamp
    }
}
