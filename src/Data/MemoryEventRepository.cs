using EventManager.Models;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace EventManager.Data
{
    public class MemoryEventRepository : IEventRepository
    {
        private static ConcurrentDictionary<Guid,Event> _events = new ConcurrentDictionary<Guid, Event>();
        public ReadOnlyCollection<Event> GetAll() => _events.Values.ToList().AsReadOnly();
        public void Add(Event e) => _events.TryAdd(e.Id, e);
        public void Update(Event e) => _events.TryUpdate(e.Id, e, _events[e.Id]); 
        public bool Remove(Guid id) => _events.Remove(id, out Event? @event);

    }

    public interface IEventRepository
    {
        ReadOnlyCollection<Event> GetAll();

        void Add(Event e);

        bool Remove(Guid id);
    }
}
