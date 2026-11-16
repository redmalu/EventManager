using EventManager.Data;
using EventManager.Models.Enums;

namespace EventManager.Application
{
    public class BookingProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<BookingProcessor> logger;

        public BookingProcessor(IServiceScopeFactory scopeFactory, ILogger<BookingProcessor> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var scope = scopeFactory.CreateScope();
                var bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();
                var allBooking = bookingRepository.GetAll();
                var pendingBooking = allBooking.Where(a => a.Status == BookingStatus.Pending).ToList();
                
                foreach(var booking in pendingBooking)
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                        booking.Confirm();
                        bookingRepository.Update(booking);
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Ошибка обработки бронирования {booking.Id}");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
            
        }
    }
}
