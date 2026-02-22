using Cars24API.Models;
using Cars24API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cars24API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;
        private readonly UserService _userService;
        private readonly CarService _carService;

        public BookingController(
            BookingService bookingService,
            UserService userService,
            CarService carService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _carService = carService;
        }

        public class BookingDto
        {
            public Booking Booking { get; set; } = null!;
            public Car? Car { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            [FromQuery] string userId,
            [FromBody] Booking booking)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId is required.");

            await _bookingService.CreateAsync(booking);

            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            user.BookingId ??= new List<string>();
            user.BookingId.Add(booking.Id);

            if (user.Id != null)
                await _userService.UpdateAsync(user.Id, user);

            return CreatedAtAction(nameof(GetBookingById),
                new { id = booking.Id }, booking);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpGet("user/{userId}/bookings")]
        public async Task<IActionResult> GetBookingsByUserId(string userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var results = new List<BookingDto>();

            if (user.BookingId == null)
                return Ok(results);

            foreach (var bookingId in user.BookingId)
            {
                var booking = await _bookingService.GetByIdAsync(bookingId);
                if (booking != null)
                {
                    var car = await _carService.GetByIdAsync(booking.CarId);

                    results.Add(new BookingDto
                    {
                        Booking = booking,
                        Car = car
                    });
                }
            }

            return Ok(results);
        }
    }
}
