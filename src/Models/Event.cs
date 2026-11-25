using System.Diagnostics.CodeAnalysis;

namespace EventManager.Models
{
    public class Event
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required DateTime StartAt { get; set; }
        public required DateTime EndAt { get; set; }
        public required int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }

        [SetsRequiredMembers]
        public Event(int totalSeats)
        {
            TotalSeats = totalSeats;
            AvailableSeats = totalSeats;
        }   

        public bool TryReserveSeats(int count = 1)
        {
            if (AvailableSeats < count)
                return false;
            AvailableSeats -= count;
            return true;
        }

        public void ReleaseSeats(int count = 1)
        {
            if (count > 0)
                AvailableSeats += count;
        }
    }
}
