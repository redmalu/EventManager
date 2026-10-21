using EventManager.Application;
using EventManager.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, MemoryEventRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => // Первым аргументом указывается название политики
    {
        policy.AllowAnyOrigin()      // Разрешить запросы от любых доменов
              .AllowAnyMethod()      // Разрешить любые HTTP-методы (GET, POST, PUT и т. д.)
              .AllowAnyHeader();     // Разрешить любые заголовки в запросе
    });
});
builder.Services.AddProblemDetails();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
