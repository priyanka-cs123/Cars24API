using Cars24API.Models;
using Cars24API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;

namespace Cars24API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly CarService _carService;

        public CarController(CarService carService)
        {
            _carService = carService;
        }

        // ✅ Get All Cars
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _carService.GetAllAsync();
            return Ok(cars);
        }

        // ✅ Get By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var car = await _carService.GetByIdAsync(id);

            if (car == null)
                return NotFound();

            return Ok(car);
        }

        // ✅ Create Car
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Car car)
        {
            if (car == null)
                return BadRequest("Car data required.");

            await _carService.CreateAsync(car);

            return CreatedAtAction(nameof(GetById),
                new { id = car.Id }, car);
        }
    }
}
