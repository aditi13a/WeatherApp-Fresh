using Microsoft.AspNetCore.Mvc;
using WeatherApp.Server.Models;
using WeatherApp.Server.Services;

namespace WeatherApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavoritesController : ControllerBase
    {
private readonly FavoritesService _service;
private readonly FavoritesService _favoritesService;

public FavoritesController(FavoritesService favoritesService)
{
    _service = favoritesService;
    _favoritesService = favoritesService;
}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var favorites = await _service.GetAsync();
            return Ok(favorites);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Favorite favorite)
        {
            if (string.IsNullOrWhiteSpace(favorite.Id))
                favorite.Id = Guid.NewGuid().ToString();

            await _service.CreateAsync(favorite);
            return Ok();
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }


        

        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromBody] Favorite fav)
        {
            if (string.IsNullOrWhiteSpace(fav.City))
                return BadRequest("City required.");

            var existing = await _favoritesService.GetByCityAsync(fav.City);

            if (existing is not null)
            {
                await _favoritesService.DeleteAsync(existing.Id);
                return Ok(new { removed = true });
            }

            fav.Id = Guid.NewGuid().ToString();
            fav.AddedAt = DateTime.UtcNow;
            await _favoritesService.CreateAsync(fav);
            return Ok(new { removed = false });
        }

    }
}
