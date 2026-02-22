using Cars24API.Models;
using MongoDB.Driver;

namespace Cars24API.Services
{
    

    public class AppointmentService
    {
        private readonly List<Appointment> appointments = new();

        public Task<List<Appointment>> GetAllAsync()
        {
            return Task.FromResult(appointments.ToList());
        }

        public Task<Appointment?> GetByIdAsync(string id)
        {
            var appointment = appointments.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(appointment);
        }

        public Task CreateAsync(Appointment appointment)
        {
            appointment.Id = Guid.NewGuid().ToString();
            appointments.Add(appointment);
            return Task.CompletedTask;
        }
    }
}
