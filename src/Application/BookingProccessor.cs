using EventManager.Data;
using EventManager.Models.Enums;

namespace EventManager.Application
{
    public class BookingProccessor : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public BookingProccessor(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
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
                    await Task.Delay(TimeSpan.FromSeconds(2),stoppingToken);
                    booking.Status = BookingStatus.Confirmed;
                    booking.ProcessedAt = DateTime.UtcNow;
                    bookingRepository.Update(booking);
                }

                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
            
        }
    }
}
