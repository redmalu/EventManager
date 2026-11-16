using EventManager.Models;
using EventManager.Models.Enums;
using FluentAssertions;

namespace TestEventManager
{
    public class BookingTests
    {
        [Fact]
        public void CreatePending_ShouldCreateBookingWithCorrectState()
        {
            var eventId = Guid.NewGuid();

            var booking = Booking.CreatePending(eventId);

            booking.Id.Should().NotBeEmpty();
            booking.EventId.Should().Be(eventId);
            booking.Status.Should().Be(BookingStatus.Pending);
            booking.ProcessedAt.Should().BeNull();
            (DateTime.UtcNow - booking.CreatedAt).TotalSeconds.Should().BeLessThan(1);
        }

        [Fact]
        public void CreatePending_MultipleCalls_ShouldGenerateDifferentIds()
        {
            var booking1 = Booking.CreatePending(Guid.NewGuid());
            var booking2 = Booking.CreatePending(Guid.NewGuid());

            booking1.Id.Should().NotBe(booking2.Id);
        }

        [Fact]
        public void Confirm_WhenPending_ShouldChangeStatusToConfirmed()
        {
            var booking = Booking.CreatePending(Guid.NewGuid());

            booking.Confirm();

            booking.Status.Should().Be(BookingStatus.Confirmed);
            booking.ProcessedAt.Should().NotBeNull();
            (DateTime.UtcNow - booking.ProcessedAt.Value).TotalSeconds.Should().BeLessThan(1);
        }

        [Fact]
        public void Confirm_WhenConfirmed_ShouldThrowInvalidOperationException()
        {
            var booking = Booking.CreatePending(Guid.NewGuid());
            booking.Confirm();

            var act = () => booking.Confirm();

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("*Pending*");
        }

        [Fact]
        public void Confirm_WhenRejected_ShouldThrowInvalidOperationException()
        {
            var booking = Booking.CreatePending(Guid.NewGuid());
            booking.Reject();

            var act = () => booking.Confirm();

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Reject_WhenPending_ShouldChangeStatusToRejected()
        {
            var booking = Booking.CreatePending(Guid.NewGuid());

            booking.Reject();

            booking.Status.Should().Be(BookingStatus.Rejected);
            booking.ProcessedAt.Should().NotBeNull();
        }

        [Fact]
        public void Reject_WhenConfirmed_ShouldThrowInvalidOperationException()
        {
            var booking = Booking.CreatePending(Guid.NewGuid());
            booking.Confirm();

            var act = () => booking.Reject();

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Reject_WhenAlreadyRejected_ShouldThrowInvalidOperationException()
        {
            var booking = Booking.CreatePending(Guid.NewGuid());
            booking.Reject();

            var act = () => booking.Reject();

            act.Should().Throw<InvalidOperationException>();
        }
    }
}

