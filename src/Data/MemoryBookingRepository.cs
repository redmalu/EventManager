using EventManager.Models;
using EventManager.Models.Enums;

namespace EventManager.Data
{
    public class MemoryBookingRepository : IBookingRepository
    {
        private List<Booking> _list = new[];

        public Booking? Get(Guid id)
        {
            return _list.FirstOrDefault(a => a.Id == id);
        }
        public List<Booking> GetByEventId(Guid eventId)
        {
            return _list.Where(a => a.EventId == eventId).OrderBy(a => a.CreatedAt).ToList();
        }

        public Booking Add(Booking booking)
        {
            _list.Add(booking);
        }

        public Booking Delete(Booking booking)
        {
            _list.Remove(booking);
        }

        public Booking Update(Booking booking)
        {
            
        }

       
    }

    public interface IBookingRepository
    {
        public Booking? Get(Guid id);
        public List<Booking> GetByEventId(Guid eventId);
        public Booking Add(Booking booking);
        public Booking Update(Booking booking);
        public Booking Delete(Booking booking);
    }
}
