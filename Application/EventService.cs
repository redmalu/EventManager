using EventManager.Data;
using EventManager.Models;
using EventManager.Models.DTO;
using System.Collections;

namespace EventManager.Application
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _repository;
        public EventService(IEventRepository repository) 
        {
            _repository = repository;
            
        }

        public Event? GetEvent(int id)
        {
            return _repository.GetAll().FirstOrDefault(ev => ev.Id == id); 
        }

        public PaginatedResult<Event> GetAllEvents(int page, int pageSize, string? title = null, DateTime? from = null, DateTime? to = null)
        {
            IEnumerable<Event> filtered = _repository.GetAll();

            if (!string.IsNullOrEmpty(title))
            {
                title = title.ToLowerInvariant();
                filtered = filtered.Where(ev => ev.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }

            if (from != null)
                filtered = filtered.Where(ev => ev.StartAt >= from);

            if(to != null)
                filtered = filtered.Where(ev => ev.EndAt <= to);

            int totalCount = filtered.Count();            
            return new PaginatedResult<Event>
            {
                Items = [.. filtered.Skip((page - 1) * pageSize).Take(pageSize)],
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        public Event CreateEvent(EventDTO eventDTO)
        {
            var _events = _repository.GetAll();
            var id = _events.Count() > 0 ? _events.Max(a => a.Id) + 1 : 1;
            var createdEvent = new Event()
            {
                Id = id,
                Title = eventDTO.Title,
                Description = eventDTO.Description,
                StartAt = eventDTO.StartAt,
                EndAt = eventDTO.EndAt
            };
            _repository.Add(createdEvent);
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
            _repository.Remove(existingEvent);
            return true;
        }
    }
}
