using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IEnumerable.Booking
{
    public class EventBookingDB_IENU : IEventBookingDB
    {
        public bool CreateEventBookingAsync(IEventBooking eventBooking)
        {
            throw new NotImplementedException();
        }

        public bool DeleteEventBookingAsync(int eventBookingID)
        {
            throw new NotImplementedException();
        }

        public List<IEventBooking> GetAllEventBookingsAsync()
        {
            throw new NotImplementedException();
        }

        public IEventBooking GetEventBookingById(int eventBookingID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEventBookingAsync(IEventBooking eventBooking)
        {
            throw new NotImplementedException();
        }
    }
}
