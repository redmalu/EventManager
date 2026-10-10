using EventManager.Models;
using EventManager.Models.DTO;

namespace EventManager.Application
{
    public class EventService : IEventService
    {
        private List<Event> _events = new();

        public Event? GetEvent(int id)
        {
            return _events.FirstOrDefault(a => a.Id == id);
        }

        public List<Event> GetAllEvents()
        {
            return _events;
        }

        public Event CreateEvent(EventDTO eventDTO)
        {
            var id = _events.Any() ? _events.Max(a => a.Id) + 1 : 1;
            var createdEvent = new Event()
            {
                Id = id,
                Title = eventDTO.Title,
                Description = eventDTO.Description,
                StartAt = eventDTO.StartAt,
                EndAt = eventDTO.EndAt
            };
            _events.Add(createdEvent);
            return createdEvent;
        }

        public bool UpdateEvent(int id, EventDTO eventDTO)
        {
            var existingEvent = _events.FirstOrDefault(a => a.Id == id);
            if (existingEvent == null)
                return false;

            existingEvent.Title = eventDTO.Title;
            existingEvent.Description = eventDTO.Description;
            existingEvent.StartAt = eventDTO.StartAt;
            existingEvent.EndAt = eventDTO.EndAt;
            return true;
        }

        public bool DeleteEvent(int id)
        {
            var existingEvent = _events.FirstOrDefault(a => a.Id == id);
            if (existingEvent == null)
                return false;
            _events.Remove(existingEvent);
            return true;
        }
    }
}
