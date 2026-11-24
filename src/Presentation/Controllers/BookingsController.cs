using EventManager.Application.Services.Interfaces;
using EventManager.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost("events/{id:guid}/book")]
        [ProducesResponseType(202)]
        public async Task<IActionResult> PostBooking(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _bookingService.CreateBookingAsync(id, cancellationToken);

                return AcceptedAtAction(nameof(GetBooking), new { id = result.Id }, new CreatedBookingDTO
                {
                    EventId = result.EventId,
                    Id = result.Id,
                    Status = result.Status.ToString()
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Не удалось создать бронь для события {eventId}", id);
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Событие не найдено");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"Создание брони для события {id} отменено");
                return StatusCode(StatusCodes.Status499ClientClosedRequest);
            }
        }

        [HttpGet("bookings/{id}")]
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
