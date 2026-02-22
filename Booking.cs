using System;

namespace Cars24API.Models
{
    public class Booking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CarId { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    }
}
