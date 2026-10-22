## Event Manager API
RESTful Web API Сервис для управления мероприятиями (событиями).  Позволяет создавать, просматривать, обновлять и удалять информацию о событиях.  
Реализовано на ASP.NET Core с хранением данных в памяти

## Возможности
- Полный CRUD (Create, Read, Update, Delete) для управления событиями.
- Архитектура, разделенная на слои (Presentation, Application, Models).
- Интеграция **Swagger/OpenAPI** для интерактивной документации.
- Централизованная обработка ошибок с поддержкой стандарта **Problem Details** (RFC 7807).
- Настроенный **CORS** для взаимодействия с любыми фронтенд-приложениями.
- Внедрение зависимостей (Dependency Injection) и логирование.

## Стек технологий
- **Язык**: C# 12+
- **Фреймворк**: ASP.NET Core Web API .NET 10
- **Документация**: Swagger / OpenAPI
- **Хранение данных**: In-Memory (список в памяти, для демонстрации/прототипа)
- **Архитектура**: Слоистая архитектура (Layered Architecture)

## Структура проекта
```text
EventManager/
├── EventManager.Application/       # Бизнес-логика (Сервисы, интерфейсы)
│   ├── EventService.cs
│   └── IEventService.cs
├── EventManager.Models/            # Модели данных и DTO
│   ├── Event.cs
│   └── DTO/
│       └── EventDTO.cs
├── EventManager.Presentation/      # Контроллеры и точка входа
│   ├── Controllers/
│   │   └── EventsController.cs
│   └── Program.cs                  # Конфигурация приложения
└── README.md
```

## Запуск проекта
### Требования
- [.NET SDK](https://dotnet.microsoft.com/download)
- IDE: Visual Studio 2022, JetBrains Rider или VS Code

### Инструкции
1. Клонируйте репозиторий:
   ```bash
   git clone `https://github.com/redmalu/EventManager`
   cd EventManager
   ```
2. Восстановите пакеты NuGet:
   ```bash
   dotnet restore
   ```
3. Запустите приложение:
   ```bash
   dotnet run
   ```
4. Откройте браузер и перейдите по адресу Swagger UI:
   - `http://localhost:5112/swagger` или `https://localhost:5112/swagger`

## API Endpoints
 - GET '/api/events'- Получить отфильтрованный и пагинированный список всех событий.
	- title string Фильтр по названию (case-insensitive, частичное совпадение)
	- to datetime События, начинающиеся не ранее указанной даты
	- from datetime События, заканчивающиеся не позднее указанной даты
	- page int Номер страницы (≥ 1) - gj по умолчанию 1
	- pageSize int Размер страницы (от 5 до 100) по умолчанию 10
 - GET '/api/events/{id}' - Получить событие по ID. 
 - POST '/api/events' - Создать новое событие.
 - PUT '/api/events/{id}' - Обновить существующее событие.
 - DELETE '/api/events/{id}' - Удалить событие по ID.

### Пример запроса (POST /api/events)
**Request Body:**
```json
{
  "title": "Конференция по .NET",
  "description": "Ежегодная встреча разработчиков",
  "startAt": "2024-11-15T10:00:00Z",
  "endAt": "2024-11-15T18:00:00Z"
}
```
**Response (201 Created):**
```json
{
  "id": 1,
  "title": "Конференция по .NET",
  "description": "Ежегодная встреча разработчиков",
  "startAt": "2024-11-15T10:00:00Z",
  "endAt": "2024-11-15T18:00:00Z"
}
```
