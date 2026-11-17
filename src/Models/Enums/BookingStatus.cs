namespace EventManager.Models.Enums
{
    public enum BookingStatus
    {
        //бронь создана, ожидает обработки
        Pending,
        
        //бронь подтверждена
        Confirmed,
        
        //бронь отклонена
        Rejected
    }
}
