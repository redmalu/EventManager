using EventManager.Models;
using EventManager.Models.Enums;

namespace EventManager.Application
{
    public interface IBookingService
    {
        public Task<Guid> CreateBookingAsync(Guid eventId);

        public Task<Booking> GetBookingByIdAsync(Guid bookingId);
    }
}
