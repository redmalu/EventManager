using EventManager.Data;
using EventManager.Models;
using EventManager.Models.Enums;

namespace EventManager.Application
{
    public class BookingService : IBookingService
    {
        private readonly ILogger<BookingService> _logger;
        private readonly IBookingRepository _repository;

        public BookingService(ILogger<BookingService> logger, IBookingRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Guid> CreateBookingAsync(Guid eventId)
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                CreatedAt = DateTime.UtcNow,
                Status = BookingStatus.Pending
            };
            _repository.Add(booking);

            return booking.Id;
        }

        public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            return _repository.Get(bookingId);
        }
    }
}
