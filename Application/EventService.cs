using EventManager.Models;

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

        public Event CreateEvent(string title, string description, DateTime startAt, DateTime endAt)
        {
            var id = _events.Any() ? _events.Max(a => a.Id) + 1 : 1;
            var createdEvent = new Event()
            {
                Id = id,
                Title = title,
                Description = description,
                StartAt = startAt,
                EndAt = endAt
            };
            _events.Add(createdEvent);
            return createdEvent;
        }

        public Event UpdateEvent()
        {
            throw new NotImplementedException();
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
