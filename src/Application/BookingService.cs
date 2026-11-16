using EventManager.Application.Interfaces;
using EventManager.Data;
using EventManager.Models;

namespace EventManager.Application
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IEventService _eventService;

        public BookingService(IBookingRepository repository, IEventService eventService)
        {
            _repository = repository;
            _eventService = eventService;
        }

        public async Task<Booking> CreateBookingAsync(Guid eventId, CancellationToken cancellationToken)
        {
            var @event = _eventService.GetEvent(eventId);
            if (@event == null) 
                throw new ArgumentException($"Событие с id = {eventId} не найдено");

            cancellationToken.ThrowIfCancellationRequested();

            var booking = Booking.CreatePending(eventId);
            _repository.Add(booking);

            return booking;
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken)
        {
            return _repository.Get(bookingId);
        }
    }
}
