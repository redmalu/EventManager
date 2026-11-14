using EventManager.Data;
using EventManager.Models;
using EventManager.Models.Enums;
using Xunit.Sdk;

namespace EventManager.Application
{
    public class BookingService : IBookingService
    {
        private readonly ILogger<BookingService> _logger;
        private readonly IBookingRepository _repository;
        private readonly IEventService _eventService;

        public BookingService(ILogger<BookingService> logger, IBookingRepository repository, IEventService eventService)
        {
            _logger = logger;
            _repository = repository;
            _eventService = eventService;
        }

        public async Task<Booking> CreateBookingAsync(Guid eventId, CancellationToken cancellationToken)
        {
            var @event = _eventService.GetEvent(eventId);
            if (@event == null) 
                throw new ArgumentException($"Событие с id = {eventId} не найдено");

            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException(cancellationToken);

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                CreatedAt = DateTime.UtcNow,
                Status = BookingStatus.Pending
            };
            _repository.Add(booking);

            return booking;
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken)
        {
            return _repository.Get(bookingId);
        }
    }
}
