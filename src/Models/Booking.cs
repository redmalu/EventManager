using EventManager.Models.Enums;

namespace EventManager.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }

        private Booking() { }

        public static Booking CreatePending(Guid eventId)
        {
            return new Booking
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                Status = BookingStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Confirm()
        {
            if (Status != BookingStatus.Pending)
                throw new InvalidOperationException(
                    $"Нельзя подтвердить бронь со статусом {Status}. Ожидается Pending.");

            Status = BookingStatus.Confirmed;
            ProcessedAt = DateTime.UtcNow;
        }

        public void Reject()
        {
            if (Status != BookingStatus.Pending)
                throw new InvalidOperationException(
                    $"Нельзя отклонить бронь со статусом {Status}. Ожидается Pending.");

            Status = BookingStatus.Rejected;
            ProcessedAt = DateTime.UtcNow;
        }
    }
}
