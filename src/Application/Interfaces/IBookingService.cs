using EventManager.Models;

namespace EventManager.Application.Interfaces
{
    public interface IBookingService
    {
        public Task<Booking> CreateBookingAsync(Guid eventId, CancellationToken cancellationToken);

        public Task<Booking?> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken);
    }
}
