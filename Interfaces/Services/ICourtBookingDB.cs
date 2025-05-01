using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface ICourtBookingDB
    {
        public bool CreateCourtBookingAsync(ICourtBooking courtBooking);
        public bool UpdateCourtBookingAsync(ICourtBooking courtBooking);
        public bool DeleteCourtBookingAsync(int courtBookingID);
        public List<ICourtBooking> GetAllCourtBookingsAsync();
        public ICourtBooking GetCourtBookingByIDAsync(int courtBookingID);
        public List<ICourtBooking> GetCourtBookingsByCourtIDAsync(int courtID);
        public List<ICourtBooking> GetCourtBookingsByEventIDAsync(int eventID);
        public List<ICourtBooking> GetCourtBookingsByTeamIDAsync(int teamID);
        public List<ICourtBooking> GetCourtBookingsByOrganiserAsync(int memberID);
        public List<ICourtBooking> GetCourtBookingsByParticipantsAsync(int memberID);
        public bool AddPartisipant(int memberID);
        public bool RemovePartisipant(int memberID);
    }
}
