using EventManager.Application.Interfaces;
using EventManager.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Presentation.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost("events/{eventid:guid}/book")]
        public async Task<IActionResult> PostBook(Guid eventId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _bookingService.CreateBookingAsync(eventId, cancellationToken);

                return AcceptedAtAction(nameof(GetBooking), new { id = result.Id }, new CreatedBookingDTO
                {
                    EventId = result.EventId,
                    Id = result.Id,
                    Status = result.Status.ToString()
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Не удалось создать бронь для события {eventId}", eventId);
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Событие не найдено");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"Создание брони для события {eventId} отменено");
                return StatusCode(StatusCodes.Status499ClientClosedRequest);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
        {
            var result = await _bookingService.GetBookingByIdAsync(id, cancellationToken);
            if (result == null)
                return Problem(
                                detail: $"Бронирование с ID = {id} не найдено",
                                statusCode: StatusCodes.Status404NotFound,
                                title: "Бронирование не найдено");
            return Ok(result);
        }
    }
}
