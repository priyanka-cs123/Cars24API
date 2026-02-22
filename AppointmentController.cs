using Microsoft.AspNetCore.Mvc;
using Cars24API.Models;
using Cars24API.Services;

namespace Cars24API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;
        private readonly UserService _userService;
        private readonly CarService _carService;

        public AppointmentController(
            AppointmentService appointmentService,
            UserService userService,
            CarService carService)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _carService = carService;
        }

        public class AppointmentDto
        {
            public Appointment Appointment { get; set; } = null!;
            public Car? Car { get; set; }
        }

        // Create Appointment
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(
            [FromQuery] string userId,
            [FromBody] Appointment appointment)
        {
            if (appointment == null || string.IsNullOrEmpty(userId))
                return BadRequest("Invalid data");

            await _appointmentService.CreateAsync(appointment);

            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            user.AppointmentId.Add(appointment.Id);

            await _userService.UpdateAsync(user.Id!, user);

            return CreatedAtAction(
                nameof(GetAppointmentById),
                new { id = appointment.Id },
                appointment);
        }

        // Get Appointment by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(string id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);

            if (appointment == null)
                return NotFound();

            return Ok(appointment);
        }

        // Get all appointments of user
        [HttpGet("user/{userId}/appointments")]
        public async Task<IActionResult> GetAppointmentsByUserId(string userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            var results = new List<AppointmentDto>();

            foreach (var appointmentId in user.AppointmentId)
            {
                var appointment = await _appointmentService.GetByIdAsync(appointmentId);
                if (appointment != null)
                {
                    var car = await _carService.GetByIdAsync(appointment.CarId);

                    results.Add(new AppointmentDto
                    {
                        Appointment = appointment,
                        Car = car
                    });
                }
            }

            return Ok(results);
        }
    }
}
