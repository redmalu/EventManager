using EventManager.Models;
using EventManager.Models.DTO;

namespace EventManager.Application
{
    public interface IEventService
    {
        public Event? GetEvent(int id);
        public PaginatedResult<Event> GetAllEvents(int page, int pageSize, string? title, DateTime? from, DateTime? to);
        public Event CreateEvent(EventDTO eventDTO);
        public bool TryUpdateEvent(int id, EventDTO eventDTO);
        public bool TryDeleteEvent(int id);
        
    }
}
