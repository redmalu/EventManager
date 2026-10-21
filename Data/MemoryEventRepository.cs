using EventManager.Models;

namespace EventManager.Data
{
    public class MemoryEventRepository : IEventRepository
    {
        private static List<Event> _events = [];
        public List<Event> GetAll() => _events.ToList();
        public void Add(Event e) => _events.Add(e);

        public void Remove(Event e) => _events.Remove(e);

    }

    public interface IEventRepository
    {
        List<Event> GetAll();

        void Add(Event e);

        void Remove(Event e);
    }
}
