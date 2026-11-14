using EventManager.Models;
using EventManager.Models.Enums;

namespace EventManager.Application
{
    public interface IBookingService
    {
        public Task<Booking> CreateBookingAsync(Guid eventId, CancellationToken cancellationToken);

        public Task<Booking?> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken);
    }
}
