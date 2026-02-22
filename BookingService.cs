using Cars24API.Models;
using MongoDB.Driver;

namespace Cars24API.Services
{
    
    public class BookingService
    {
        private readonly List<Booking> bookings = new();

        public Task<List<Booking>> GetAllAsync()
        {
            return Task.FromResult(bookings.ToList());
        }

        public Task<Booking?> GetByIdAsync(string id)
        {
            var booking = bookings.FirstOrDefault(b => b.Id == id);
            return Task.FromResult(booking);
        }

        public Task CreateAsync(Booking booking)
        {
            booking.Id = Guid.NewGuid().ToString();
            bookings.Add(booking);
            return Task.CompletedTask;
        }
    }
}
