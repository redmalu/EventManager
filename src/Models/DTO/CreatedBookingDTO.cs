using EventManager.Models.Enums;

namespace EventManager.Models.DTO
{
    public class CreatedBookingDTO
    {
        public required Guid Id { get; set; }
        public required Guid EventId { get; set; }
        public required string Status { get; set; }
    }
}
