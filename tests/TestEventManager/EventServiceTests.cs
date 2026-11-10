using EventManager.Application;
using EventManager.Data;
using EventManager.Models;
using EventManager.Models.DTO;
using FluentAssertions;
using Moq;
using System.Collections.ObjectModel;

namespace TestEventManager
{
    public class EventServiceTests
    {
        // Фиксированные Guid для воспроизводимости тестов
        private static readonly Guid Id1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private static readonly Guid Id2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
        private static readonly Guid Id3 = Guid.Parse("00000000-0000-0000-0000-000000000003");



        [Fact]
        [Trait("Category", "GetAll")]
        public void GetAllEvents_WhenRepositoryEmpty_ReturnEmptyList()
        {
            int pageSize = 10;
            int page = 1;
            var mockRepository = new Mock<IEventRepository>();

            mockRepository.Setup(repo => repo.GetAll()).Returns(new ReadOnlyCollection<Event>(new List<Event>()));
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize);

            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.PageSize.Should().Be(pageSize);
            result.TotalPages.Should().Be(0);
        }

        [Fact]
        [Trait("Category", "GetAll")]
        public void GetAllEvents_ReturnsAllData()
        {
            int pageSize = 10;
            int page = 1;
            var mockRepository = new Mock<IEventRepository>();
            Dictionary<Guid, Event> list = CreateTestEvents();
            mockRepository.Setup(repo => repo.GetAll()).Returns(list.Values.ToList().AsReadOnly());
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.First().Title.Should().Be("Мероприятие 1");
            result.TotalPages.Should().Be(1);
            result.Page.Should().Be(page);
        }

        private static Dictionary<Guid, Event> CreateTestEvents()
        {
            return new()
            {
                [Id1] = new Event { Id = Id1, Title = "Мероприятие 1", StartAt = new DateTime(2026,11,25), EndAt = new DateTime(2027,01,25) },
                [Id2] = new Event { Id = Id2, Title = "Мероприятие 2", StartAt = new DateTime(2026,10,15), EndAt = new DateTime(2026,12,15) }
            };
        }

        [Fact]
        [Trait("Category", "GetAll")]
        public void GetAllEvents_FilteredByTitle_ReturnsEvents()
        {
            string title = "меро";
            int pageSize = 10;
            int page = 1;
            var mockRepository = new Mock<IEventRepository>();
            Dictionary<Guid, Event> list = new()
            {
                [Id1] = new Event { Id = Id1, Title = "Мероприятие 1", StartAt = new DateTime(2026, 12, 01), EndAt = new DateTime(2026, 12, 02) },
                [Id2] = new Event { Id = Id2, Title = "Мероприятие 2", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) },
                [Id3] = new Event { Id = Id3, Title = "Конференция IT", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) }
            };
            mockRepository.Setup(repo => repo.GetAll()).Returns(list.Values.ToList().AsReadOnly());
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize, title: title);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.Should().AllSatisfy(e =>
                e.Title.ToLower().Should().Contain(title));
        }

        [Fact]
        [Trait("Category", "GetAll")]
        public void GetAllEvents_FilteredByStartAt_ReturnsEvents()
        {
            DateTime from = new(2026, 12, 02);
            int pageSize = 10;
            int page = 1;
            var mockRepository = new Mock<IEventRepository>();
            Dictionary<Guid, Event> list = new()
            {
                [Id1] = new Event { Id = Id1, Title = "Мероприятие 1", StartAt = new DateTime(2026, 12, 01), EndAt = new DateTime(2026, 12, 02) },
                [Id2] = new Event { Id = Id2, Title = "Мероприятие 2", StartAt = new DateTime(2026, 12, 10), EndAt = new DateTime(2026, 12, 22) },
                [Id3] = new Event { Id = Id3, Title = "Конференция IT", StartAt = new DateTime(2027, 12, 01), EndAt = new DateTime(2027, 12, 02) }
            };
            mockRepository.Setup(repo => repo.GetAll()).Returns(list.Values.ToList().AsReadOnly());
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize, from: from);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.Should().AllSatisfy(e =>
                e.StartAt.Should().BeAfter(from));
        }

        [Fact]
        [Trait("Category", "GetAll")]
        public void GetAllEvents_FilteredByEndAt_ReturnsEvents()
        {
            DateTime to = new(2026, 12, 22);
            int pageSize = 10;
            int page = 1;
            var mockRepository = new Mock<IEventRepository>();
            Dictionary<Guid, Event> list = new()
            {
                [Id1] = new Event { Id = Id1, Title = "Мероприятие 1", StartAt = new DateTime(2026, 12, 01), EndAt = new DateTime(2026, 12, 02) },
                [Id2] = new Event { Id = Id2, Title = "Мероприятие 2", StartAt = new DateTime(2026, 12, 10), EndAt = new DateTime(2026, 12, 22) },
                [Id3] = new Event { Id = Id3, Title = "Конференция IT", StartAt = new DateTime(2027, 12, 01), EndAt = new DateTime(2027, 12, 02) }
            };
            mockRepository.Setup(repo => repo.GetAll()).Returns(list.Values.ToList().AsReadOnly());
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize, to: to);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.Should().AllSatisfy(e =>
                e.EndAt.Should().BeOnOrBefore(to));
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
            Dictionary<Guid, Event> list = new()
            {
                [Id1] = new Event { Id = Id1, Title = "Мероприятие 1", StartAt = new DateTime(2026, 12, 01), EndAt = new DateTime(2026, 12, 02) },
                [Id2] = new Event { Id = Id2, Title = "Мероприятие 2", StartAt = new DateTime(2026, 12, 10), EndAt = new DateTime(2026, 12, 22) },
                [Id3] = new Event { Id = Id3, Title = "Конференция IT", StartAt = new DateTime(2027, 12, 01), EndAt = new DateTime(2027, 12, 02) }
            };
            mockRepository.Setup(repo => repo.GetAll()).Returns(list.Values.ToList().AsReadOnly());
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetAllEvents(page, pageSize, from: from, to: to);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);

            var resultEvent = result.Items.First();
            resultEvent.StartAt.Should().BeOnOrAfter(from);
            resultEvent.EndAt.Should().BeOnOrBefore(to);
            resultEvent.Title.ToLower().Should().Contain(title);
        }

        [Fact]
        [Trait("Category", "GetEvent")]
        public void GetEvent_WhenIdExist_ReturnEvent()
        {
            var expectedId = Id1;
            Dictionary<Guid, Event> list = CreateTestEvents();
            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(list.Values.ToList().AsReadOnly());
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetEvent(expectedId);

            result.Should().NotBeNull();
            result.Id.Should().Be(expectedId);
            result.Title.Should().Be("Мероприятие 1");
        }

        [Fact]
        [Trait("Category", "GetEvent")]
        public void GetEvent_WhenIdNotExist_ReturnNull()
        {
            var expectedId = Id3;
            Dictionary<Guid, Event> list = CreateTestEvents();
            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(list.Values.ToList().AsReadOnly());
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.GetEvent(expectedId);

            result.Should().BeNull();
        }

        [Fact]
        [Trait("Category", "CreateEvent")]
        public void CreateEvent_ReturnEvent()
        {
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

            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty(); 
            result.Title.Should().Be(expectedTitle);
        }

        [Fact]
        [Trait("Category", "UpdateEvent")]
        public void TryUpdateEvent_WhenIdExists_ReturnTrue()
        {
            var updateId = Id2;
            var updateEventDTO = new EventDTO() { Title = "БМК", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) };
            Dictionary<Guid, Event> list = CreateTestEvents();

            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(list.Values.ToList().AsReadOnly());
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.TryUpdateEvent(updateId, updateEventDTO);
            var resultEvent = eventService.GetEvent(updateId);
            result.Should().BeTrue();
            resultEvent.Should().NotBeNull();
            resultEvent.Title.Should().Be("БМК");
        }

        [Fact]
        [Trait("Category", "UpdateEvent")]
        public void TryUpdateEvent_WhenIdNotExists_ReturnFalse()
        {
            var updateId = Id3;
            var updateEventDTO = new EventDTO() { Title = "БМК", StartAt = DateTime.Today, EndAt = DateTime.Today.AddMonths(1) };
            Dictionary<Guid, Event> list = CreateTestEvents();

            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(list.Values.ToList().AsReadOnly());
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.TryUpdateEvent(updateId, updateEventDTO);
            result.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "DeleteEvent")]
        public void TryDeleteEvent_WhenIdExists_ReturnTrue()
        {
            var deletedId = Id2;
            Dictionary<Guid, Event> list = CreateTestEvents();

            var mockRepository = CreateMockRepository();
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.TryDeleteEvent(deletedId);

            var resultEvent = eventService.GetEvent(deletedId);
            result.Should().BeTrue();
            resultEvent.Should().BeNull();
        }

        [Fact]
        [Trait("Category", "DeleteEvent")]
        public void TryDeleteEvent_WhenIdNotExists_ReturnFalse()
        {
            var deletedId = Id3;
            Dictionary<Guid, Event> list = CreateTestEvents();
            Mock<IEventRepository> mockRepository = CreateMockRepository(list);
            var eventService = new EventService(mockRepository.Object);

            var result = eventService.TryDeleteEvent(deletedId);

            result.Should().BeFalse();
        }

        private static Mock<IEventRepository> CreateMockRepository(Dictionary<Guid, Event> list)
        {
            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(() => list.Values.ToList().AsReadOnly());
            mockRepository.Setup(repo => repo.Remove(It.IsAny<Guid>())).Returns<Guid>(id => list.Remove(id));
            return mockRepository;
        }
    }
}