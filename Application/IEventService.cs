using EventManager.Models;

namespace EventManager.Application
{
    public interface IEventService
    {
        public Event? GetEvent(int id);
        public List<Event> GetAllEvents();
        public Event CreateEvent(string title, string description, DateTime startAt, DateTime endAt);
        public Event UpdateEvent();
        public bool DeleteEvent(int id);
        
    }
}
