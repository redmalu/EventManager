using EventManager.Models;
using EventManager.Models.DTO;

namespace EventManager.Application
{
    public interface IEventService
    {
        public Event? GetEvent(int id);
        public List<Event> GetAllEvents();
        public Event CreateEvent(EventDTO eventDTO);
        public bool UpdateEvent(int id, EventDTO eventDTO);
        public bool DeleteEvent(int id);
        
    }
}
