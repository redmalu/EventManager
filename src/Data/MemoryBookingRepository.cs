using EventManager.Models;
using EventManager.Models.Enums;
using System.Collections.Concurrent;

namespace EventManager.Data
{
    public class MemoryBookingRepository : IBookingRepository
    {
        private ConcurrentDictionary<Guid, Booking> _list = new();

        public Booking? Get(Guid id)
        {
            return _list.TryGetValue(id, out var booking) ? booking : null;
        }
        public List<Booking> GetByEventId(Guid eventId)
        {
            return _list.Values.Where(a => a.EventId == eventId).OrderBy(a => a.CreatedAt).ToList();
        }

        public bool Add(Booking booking)
        {
            return _list.TryAdd(booking.Id, booking);
        }

        public bool Delete(Booking booking)
        {
            return _list.TryRemove(booking.Id, out Booking curBooking);
        }

        public bool Update(Booking booking)
        {
            return _list.TryUpdate(booking.Id, booking, _list[booking.Id]);
        }
    }

    public interface IBookingRepository
    {
        public Booking? Get(Guid id);
        public List<Booking> GetByEventId(Guid eventId);
        public bool Add(Booking booking);
        public bool Update(Booking booking);
        public bool Delete(Booking booking);
    }
}
