using EventManager.Models;
using EventManager.Models.DTO;

namespace EventManager.Application
{
    public class EventService : IEventService
    {
        private Dictionary<int, Event> _events = [];

        public Event? GetEvent(int id)
        {
            _events.TryGetValue(id, out Event? currentEvent);
            return currentEvent;
        }

        public List<Event> GetAllEvents()
        {
            return [.. _events.Values];
        }

        public Event CreateEvent(EventDTO eventDTO)
        {
            var id = _events.Count > 0 ? _events.Max(a => a.Key) + 1 : 1;
            var createdEvent = new Event()
            {
                Id = id,
                Title = eventDTO.Title,
                Description = eventDTO.Description,
                StartAt = eventDTO.StartAt,
                EndAt = eventDTO.EndAt
            };
            _events.Add(id, createdEvent);
            return createdEvent;
        }

        public bool TryUpdateEvent(int id, EventDTO eventDTO)
        {
            var existingEvent = GetEvent(id);
            if (existingEvent == null)
                return false;

            existingEvent.Title = eventDTO.Title;
            existingEvent.Description = eventDTO.Description;
            existingEvent.StartAt = eventDTO.StartAt;
            existingEvent.EndAt = eventDTO.EndAt;
            return true;
        }

        public bool TryDeleteEvent(int id)
        {
            var existingEvent = GetEvent(id);
            if (existingEvent == null)
                return false;
            _events.Remove(id);
            return true;
        }
    }
}
