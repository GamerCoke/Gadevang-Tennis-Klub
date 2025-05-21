using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface IEventBookingDB
    {
        Task<List<IEventBooking>> GetAllEventBookingsAsync();
        Task<bool> CreateEventBookingAsync(IEventBooking eventBooking);
        Task<bool> DeleteEventBookingAsync(int eventBookingID);
        Task<IEventBooking> GetEventBookingById(int eventBookingID);
        Task<List<IEventBooking>> GetEventBookingsByMemberIDAsync(int memberID);
        Task<List<IEventBooking>> GetEventBookingsByEventIDAsync(int eventID);
    }
}
