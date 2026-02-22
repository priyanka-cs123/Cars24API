using Microsoft.AspNetCore.Mvc;
using Cars24API.Services;

namespace Cars24API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarSearchController : ControllerBase
    {
        private readonly CarService _carService;

        public CarSearchController(CarService carService)
        {
            _carService = carService;
        }

        // 🔎 ADVANCED SEARCH
        // Example:
        // /api/CarSearch/search?keyword=honda&fuel=Petrol&minYear=2020
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? keyword,
            [FromQuery] string? fuel,
            [FromQuery] string? transmission,
            [FromQuery] int? minYear,
            [FromQuery] int? maxYear,
            [FromQuery] int? minMileage,
            [FromQuery] int? maxMileage)
        {
            var results = await _carService.AdvancedSearchAsync(
                keyword,
                fuel,
                transmission,
                minYear,
                maxYear,
                minMileage,
                maxMileage);

            return Ok(results);
        }

        // 💡 AUTO SUGGESTIONS
        // Example:
        // /api/CarSearch/suggestions?keyword=hon
        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Keyword is required");

            var suggestions = await _carService.GetSuggestionsAsync(keyword);

            return Ok(suggestions);
        }
    }
}