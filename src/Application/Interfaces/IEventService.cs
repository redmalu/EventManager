using EventManager.Models;
using EventManager.Models.DTO;

namespace EventManager.Application.Interfaces
{
    public interface IEventService
    {
        public Event? GetEvent(Guid id);
        public PaginatedResult<Event> GetAllEvents(int page, int pageSize, string? title, DateTime? from, DateTime? to);
        public Event CreateEvent(EventDTO eventDTO);
        public bool TryUpdateEvent(Guid id, EventDTO eventDTO);
        public bool TryDeleteEvent(Guid id);
        
    }
}
