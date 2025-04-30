using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface IEventBookingDB
    {
        List<IEventBooking> GetAllEventBookingsAsync();
        bool CreateEventBookingAsync(IEventBooking eventBooking);
        bool DeleteEventBookingAsync(int eventBookingID);
        bool UpdateEventBookingAsync(IEventBooking eventBooking);
        IEventBooking GetEventBookingById(int eventBookingID);
    }
}
