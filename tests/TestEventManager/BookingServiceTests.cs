using EventManager.Application.Services;
using EventManager.Application.Services.Interfaces;
using EventManager.Data;
using EventManager.Models;
using EventManager.Models.Enums;
using FluentAssertions;
using Moq;

namespace TestEventManager
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IEventService> _eventServiceMock;

        public BookingServiceTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _eventServiceMock = new Mock<IEventService>();

        }

        /// <summary>
        /// создание брони для существующего события — возвращается BookingInfo со статусом Pending;
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Category", "CreateBookingAsync")]
        public async Task CreateBookingAsync_WhenEventFound_ReturnNewBooking()
        {
            var eventId = Guid.NewGuid();
            var mockEvent = CreateMockEvent(eventId);
            _eventServiceMock.Setup( a => a.GetEvent(eventId)).Returns(mockEvent);
            var bookingService = new BookingService(_bookingRepositoryMock.Object, _eventServiceMock.Object);
            var cancellationToken = CancellationToken.None;
            var result = await bookingService.CreateBookingAsync(eventId, cancellationToken);

            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
            result.EventId.Should().Be(eventId);
            result.Status.Should().Be(BookingStatus.Pending);
            (DateTime.UtcNow - result.CreatedAt).TotalSeconds.Should().BeLessThan(1);
            result.Should().BeOfType<Booking>();

            _bookingRepositoryMock.Verify(
                x => x.Add(It.Is<Booking>(b => b.EventId == eventId && b.Status == BookingStatus.Pending)),
                Times.Once);
        }

        /// <summary>
        /// создание нескольких броней для одного события — все создаются с уникальными Id;
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Category", "CreateBookingAsync")]
        public async Task CreateBookingAsync_WhenEventFoundAndCallTwice_ReturnTwoNewBookingForEvent()
        {
            var eventId = Guid.NewGuid();
            Event mockEvent = CreateMockEvent(eventId);
            _eventServiceMock.Setup(a => a.GetEvent(eventId)).Returns(mockEvent);
            var bookingService = new BookingService(_bookingRepositoryMock.Object, _eventServiceMock.Object);
            var cancellationToken = CancellationToken.None;
            var result1 = await bookingService.CreateBookingAsync(eventId, cancellationToken);
            var result2 = await bookingService.CreateBookingAsync(eventId, cancellationToken);

            result1.Should().NotBeNull();
            result1.Id.Should().NotBe(Guid.Empty);
            result1.EventId.Should().Be(eventId);
            result1.Status.Should().Be(BookingStatus.Pending);
            (DateTime.UtcNow - result1.CreatedAt).TotalSeconds.Should().BeLessThan(1);
            result1.Should().BeOfType<Booking>();

            result2.Should().NotBeNull();
            result2.Id.Should().NotBe(Guid.Empty);
            result2.EventId.Should().Be(eventId);
            result2.Status.Should().Be(BookingStatus.Pending);
            (DateTime.UtcNow - result2.CreatedAt).TotalSeconds.Should().BeLessThan(1);
            result2.Should().BeOfType<Booking>();

            result1.Id.Should().NotBe(result2.Id);

            _bookingRepositoryMock.Verify(
                x => x.Add(It.Is<Booking>(b => b.EventId == eventId && b.Status == BookingStatus.Pending)),
                Times.Exactly(2));
        }

        private static Event CreateMockEvent(Guid eventId)
        {
            return new Event(100) { Id = eventId, Title = "Мероприятие 1", Description = "Концерт", StartAt = new DateTime(2026, 12, 12), EndAt = new DateTime(2026, 12, 13) };
        }

        /// <summary>
        /// создание брони для несуществующего события;
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Category", "CreateBookingAsync")]
        public async Task CreateBookingAsync_WhenEventNotFound_ReturnException()
        {
            var eventId = Guid.NewGuid();
            _eventServiceMock.Setup(a => a.GetEvent(eventId)).Returns((Event?)null);
            var bookingService = new BookingService(_bookingRepositoryMock.Object, _eventServiceMock.Object);
            var cancellationToken = CancellationToken.None;
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => bookingService.CreateBookingAsync(eventId, cancellationToken));

            Assert.Contains(eventId.ToString(), exception.Message);
            _bookingRepositoryMock.Verify(x => x.Add(It.IsAny<Booking>()), Times.Never);
        }

        /// <summary>
        /// получение брони по Id — возвращается корректная информация;
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Category", "GetBookingByIdAsync")]
        public async Task GetBookingByIdAsync_WhenIdExists_ReturnBooking()
        {
            var eventId = Guid.NewGuid();
            var booking = Booking.CreatePending(eventId); 
            _bookingRepositoryMock.Setup(a => a.Get(It.IsAny<Guid>())).Returns(booking);
            var bookingService = new BookingService(_bookingRepositoryMock.Object, _eventServiceMock.Object);
            var cancellationToken = CancellationToken.None;
            var result = await bookingService.GetBookingByIdAsync(booking.Id, cancellationToken);

            result.Should().NotBeNull();
            result.Id.Should().Be(booking.Id);
            result.Status.Should().Be(BookingStatus.Pending);
            
        }
        //Успешные сценарии:
        //
        //
        //
        //получение брони отражает изменение статуса(после Confirm/Reject).

        //Неуспешные сценарии:
        //
        //создание брони для удалённого события;
        //получение брони по несуществующему Id.
    }
}
