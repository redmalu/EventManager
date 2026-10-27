using EventManager.Application;
using EventManager.Models;
using EventManager.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Presentation.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<PaginatedResult<Event>> GetAll(string? title, DateTime? from, DateTime? to, int? page = 1, int? pageSize = 10)
        {
            if (page < 1)
                return BadRequest("Номер страницы не должен быть меньше 1");
            else if ((pageSize < 5 || pageSize > 100))
                return BadRequest("Размер страницы не должен быть меньше 5 и больше 100");
            
            return Ok(_eventService.GetAllEvents(page!.Value, pageSize!.Value, title, from, to));
        }

        [HttpGet("{id}")]
        public ActionResult<Event> Get(int id) 
        {
            var _event = _eventService.GetEvent(id);
            if (_event == null)
                return NotFound($"Событие с ID = {id} не найдено");
            return Ok(_event);    
        }
        
        [HttpPost]
        public IActionResult Post(EventDTO eventDTO) 
        {
            var currentEvent = _eventService.CreateEvent(eventDTO);
            return CreatedAtAction(nameof(Get), new { id = currentEvent.Id }, currentEvent);
        }
        
        [HttpPut("{id}")]
        public IActionResult Put(int id, EventDTO eventDTO) 
        {
            var result = _eventService.TryUpdateEvent(id, eventDTO);
            return result ? NoContent() : NotFound($"Событие с ID = {id} не найдено");
        }

        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            var result = _eventService.TryDeleteEvent(id);
            return result ? NoContent() : NotFound($"Событие с ID = {id} не найдено");
        }
        
    }
}
