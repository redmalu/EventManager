using EventManager.Models;
using EventManager.Models.Enums;
using System.Collections.Concurrent;

namespace EventManager.Data
{
    public class MemoryBookingRepository : IBookingRepository
    {
        private static ConcurrentDictionary<Guid, Booking> _dictionary = new();

        public Booking? Get(Guid id) => _dictionary.TryGetValue(id, out var booking) ? booking : null;

        public List<Booking> GetAll() => [.. _dictionary.Values];

        public List<Booking> GetByEventId(Guid eventId) =>
            [.. _dictionary.Values.Where(a => a.EventId == eventId).OrderBy(a => a.CreatedAt)];


        public bool Add(Booking booking)
        {
            ArgumentNullException.ThrowIfNull(booking);
            return _dictionary.TryAdd(booking.Id, booking);
        }
        

        public bool Delete(Booking booking)
        {
            ArgumentNullException.ThrowIfNull(booking);
            return _dictionary.TryRemove(booking.Id, out _);
        }

        public bool Update(Booking booking)
        {
            ArgumentNullException.ThrowIfNull(booking);

            return _dictionary.TryGetValue(booking.Id, out var existing)
                && _dictionary.TryUpdate(booking.Id, booking, existing);
        }
    }

    public interface IBookingRepository
    {
        public Booking? Get(Guid id);
        public List<Booking> GetAll();
        public List<Booking> GetByEventId(Guid eventId);
        public bool Add(Booking booking);
        public bool Update(Booking booking);
        public bool Delete(Booking booking);
    }
}
