using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IEnumerable.Booking
{
    public class EventBookingDB_IENU : IEventBookingDB
    {
        public Task<bool> CreateEventBookingAsync(IEventBooking eventBooking)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEventBookingAsync(int eventBookingID)
        {
            throw new NotImplementedException();
        }

        public Task<List<IEventBooking>> GetAllEventBookingsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEventBooking> GetEventBookingById(int eventBookingID)
        {
            throw new NotImplementedException();
        }

        public Task<List<IEventBooking>> GetEventBookingsByEventIDAsync(int eventID)
        {
            throw new NotImplementedException();
        }

        public Task<List<IEventBooking>> GetEventBookingsByMemberIDAsync(int memberID)
        {
            throw new NotImplementedException();
        }
    }
}
