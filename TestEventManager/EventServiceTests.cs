using EventManager.Application;
using EventManager.Data;
using EventManager.Models;
using EventManager.Models.DTO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;

namespace TestEventManager
{
    public class EventServiceTests
    {
        [Fact]
        [Trait("Category", "GetAll")]
        public void GetAllEvents_WhenRepositoryEmpty_ReturnEmptyList()
        {
            int pageSize = 10;
            int page = 1;
            int expectedTotalCount = 0;
            var mockRepository = new Mock<IEventRepository>();
            var emptyList = new List<Event>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(emptyList);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize);
            
            Assert.Equal(emptyList, result.Items);
            Assert.Equal(expectedTotalCount, result.TotalCount);
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal(0, result.TotalPages);
            Assert.NotNull(result);
        }

        [Fact]
        [Trait("Category", "GetAll")]
        public void GetAllEvents_ReturnsAllData()
        {
            int pageSize = 10;
            int page = 1;
            var mockRepository = new Mock<IEventRepository>();
            List<Event> list = [ 
                new Event { Id = 1, Title = "Мероприятие 1", StartAt = DateTime.Today.AddDays(-2), EndAt = DateTime.Today.AddMonths(2) },
                new Event { Id = 2, Title = "Мероприятие 2", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) }
            ];
            mockRepository.Setup(repo => repo.GetAll()).Returns(list);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize);
            
            Assert.Equal(2, result.Items.Count);
            Assert.Equal("Мероприятие 1", result.Items.First().Title);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(page, result.Page);
        }    

        [Fact]
        [Trait("Category", "GetAll")]
        public void GetAllEvents_FilteredByTitle_ReturnsEvents()
        {
            string title = "меро";
            int pageSize = 10;
            int page = 1;
            var mockRepository = new Mock<IEventRepository>();
            List<Event> list = [
                new Event { Id = 1, Title = "Мероприятие 1", StartAt = new DateTime(2026, 12, 01), EndAt = new DateTime(2026,12,02) },
                new Event { Id = 2, Title = "Мероприятие 2", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) },
                new Event { Id = 3, Title = "Конференция IT", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) }
            ];
            mockRepository.Setup(repo => repo.GetAll()).Returns(list);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize, title: title);

            Assert.Equal(2, result.Items.Count);
            Assert.Contains(title, result.Items.First().Title.ToLower());
            Assert.Contains(title, result.Items.Last().Title.ToLower());
        }

        [Fact]
        [Trait("Category", "GetAll")]
        public void GetAllEvents_FilteredByStartAt_ReturnsEvents()
        {
            DateTime from = new(2026, 12, 02);
            int pageSize = 10;
            int page = 1;
            var mockRepository = new Mock<IEventRepository>();
            List<Event> list = [
                new Event { Id = 1, Title = "Мероприятие 1", StartAt = new DateTime(2026, 12, 01), EndAt = new DateTime(2026,12,02) },
                new Event { Id = 2, Title = "Мероприятие 2", StartAt = new DateTime(2026, 12, 10), EndAt = new DateTime(2026,12,22) },
                new Event { Id = 3, Title = "Конференция IT", StartAt = new DateTime(2027, 12, 01), EndAt = new DateTime(2027,12,02) }
            ];
            mockRepository.Setup(repo => repo.GetAll()).Returns(list);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize, from: from);

            Assert.Equal(2, result.Items.Count);
            Assert.True( from < result.Items.First().StartAt);
            Assert.True( from < result.Items.Last().StartAt);
        }

        [Fact]
        [Trait("Category", "GetAll")]
        public void GetAllEvents_FilteredByEndAt_ReturnsEvents()
        {
            DateTime to = new(2026, 12, 22);
            int pageSize = 10;
            int page = 1;
            var mockRepository = new Mock<IEventRepository>();
            List<Event> list = [
                new Event { Id = 1, Title = "Мероприятие 1", StartAt = new DateTime(2026, 12, 01), EndAt = new DateTime(2026,12,02) },
                new Event { Id = 2, Title = "Мероприятие 2", StartAt = new DateTime(2026, 12, 10), EndAt = new DateTime(2026,12,22) },
                new Event { Id = 3, Title = "Конференция IT", StartAt = new DateTime(2027, 12, 01), EndAt = new DateTime(2027,12,02) }
            ];
            mockRepository.Setup(repo => repo.GetAll()).Returns(list);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize, to: to);

            Assert.Equal(2, result.Items.Count);
            Assert.True(to >= result.Items.First().EndAt);
            Assert.True(to >= result.Items.Last().EndAt);
        }

        [Fact]
        [Trait("Category", "GetAll")]
        public void GetAllEvents_FilteredByStartAt_EndAt_Title_ReturnsEvents()
        {
            string title = "меро";
            DateTime from = new(2026, 12, 02);
            DateTime to = new(2026, 12, 22);
            int pageSize = 10;
            int page = 1;
            var mockRepository = new Mock<IEventRepository>();
            List<Event> list = [
                new Event { Id = 1, Title = "Мероприятие 1", StartAt = new DateTime(2026, 12, 01), EndAt = new DateTime(2026,12,02) },
                new Event { Id = 2, Title = "Мероприятие 2", StartAt = new DateTime(2026, 12, 10), EndAt = new DateTime(2026,12,22) },
                new Event { Id = 3, Title = "Конференция IT", StartAt = new DateTime(2027, 12, 01), EndAt = new DateTime(2027,12,02) }
            ];
            mockRepository.Setup(repo => repo.GetAll()).Returns(list);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize, from: from, to: to);

            Assert.Equal(1, result.Items.Count);
            var resultEvent = result.Items.First();
            Assert.True(from <= resultEvent.StartAt);
            Assert.True(to >= resultEvent.EndAt);
            Assert.Contains(title, resultEvent.Title.ToLower());
        }

        [Fact]
        [Trait("Category", "GetEvent")]
        public void GetEvent_WhenIdExist_ReturnEvent()
        {
            int expectedId = 1;
            var mockRepository = new Mock<IEventRepository>();
            List<Event> list = [
                new Event { Id = 1, Title = "Мероприятие 1", StartAt = DateTime.Today.AddDays(-2), EndAt = DateTime.Today.AddMonths(2) },
                new Event { Id = 2, Title = "Мероприятие 2", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) }
            ];
            mockRepository.Setup(repo => repo.GetAll()).Returns(list);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetEvent(expectedId);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.Equal("Мероприятие 1", result.Title);
        }

        [Fact]
        [Trait("Category", "GetEvent")]
        public void GetEvent_WhenIdNotExist_ReturnNull()
        {
            int expectedId = 3;
            var mockRepository = new Mock<IEventRepository>();
            List<Event> list = [
                new Event { Id = 1, Title = "Мероприятие 1", StartAt = DateTime.Today.AddDays(-2), EndAt = DateTime.Today.AddMonths(2) },
                new Event { Id = 2, Title = "Мероприятие 2", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) }
            ];
            mockRepository.Setup(repo => repo.GetAll()).Returns(list);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetEvent(expectedId);

            Assert.Null(result);
        }

        [Fact]
        [Trait("Category", "CreateEvent")]
        public void CreateEvent_ReturnEvent()
        {
            int expectedId = 1;
            string expectedTitle = "Конференция IT";
            EventDTO eventDTO = new() 
            { 
                Title = expectedTitle, 
                Description = "Большая конфа", 
                StartAt = new DateTime(2026, 11, 01),
                EndAt = new DateTime(2026, 12, 01)
            };

            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns([]);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.CreateEvent(eventDTO);

            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.Equal(expectedTitle, result.Title);
        }

       

        [Fact]
        [Trait("Category", "UpdateEvent")]
        public void TryUpdateEvent_WhenIdExists_ReturnTrue()
        {
            int updateId = 2;
            var updateEventDTO = new EventDTO() { Title = "БМК", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) };
            List<Event> list = [
                new Event { Id = 1, Title = "Мероприятие 1", StartAt = DateTime.Today.AddDays(-2), EndAt = DateTime.Today.AddMonths(2) },
                new Event { Id = 2, Title = "Мероприятие 2", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) }
            ];

            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(list);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.TryUpdateEvent(updateId, updateEventDTO);
            var resultEvent = eventService.GetEvent(updateId);
            Assert.True(result);
            Assert.NotNull(resultEvent);
            Assert.Equal("БМК", resultEvent.Title);
        }

        [Fact]
        [Trait("Category", "UpdateEvent")]
        public void TryUpdateEvent_WhenIdNotExists_ReturnFalse()
        {
            int updateId = 3;
            var updateEventDTO = new EventDTO() { Title = "БМК", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) };
            List<Event> list = [
                new Event { Id = 1, Title = "Мероприятие 1", StartAt = DateTime.Today.AddDays(-2), EndAt = DateTime.Today.AddMonths(2) },
                new Event { Id = 2, Title = "Мероприятие 2", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) }
            ];

            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(list);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.TryUpdateEvent(updateId, updateEventDTO);
            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "DeleteEvent")]
        public void TryDeleteEvent_WhenIdExists_ReturnTrue()
        {
            int deletedId = 2;
            List<Event> list = [
                new Event { Id = 1, Title = "Мероприятие 1", StartAt = DateTime.Today.AddDays(-2), EndAt = DateTime.Today.AddMonths(2) },
                new Event { Id = 2, Title = "Мероприятие 2", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) }
            ];

            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(list);
            mockRepository.Setup(repo => repo.Remove(It.IsAny<Event>())).Callback<Event>(e => list.Remove(e));
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.TryDeleteEvent(deletedId);

            var resultEvent = eventService.GetEvent(deletedId);
            Assert.True(result);
            Assert.Null(resultEvent);
        }

        [Fact]
        [Trait("Category", "DeleteEvent")]
        public void TryDeleteEvent_WhenIdNotExists_ReturnFalse()
        {
            int deletedId = 3;
            List<Event> list = [
                new Event { Id = 1, Title = "Мероприятие 1", StartAt = DateTime.Today.AddDays(-2), EndAt = DateTime.Today.AddMonths(2) },
                new Event { Id = 2, Title = "Мероприятие 2", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) }
            ];

            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(list);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.TryDeleteEvent(deletedId);

            Assert.False(result);
        }
    }
}
