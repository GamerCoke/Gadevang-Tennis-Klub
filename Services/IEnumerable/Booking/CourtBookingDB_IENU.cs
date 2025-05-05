using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IENumerable.Booking
{
    public class CourtBookingDB_IENU : ICourtBookingDB
    {
        public bool CreateCourtBookingAsync(ICourtBooking courtBooking)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCourtBookingAsync(int courtBookingID)
        {
            throw new NotImplementedException();
        }

        public List<ICourtBooking> GetAllCourtBookingsAsync()
        {
            throw new NotImplementedException();
        }

        public ICourtBooking GetCourtBookingByIDAsync(int courtBookingID)
        {
            throw new NotImplementedException();
        }

        public List<ICourtBooking> GetCourtBookingsByCourtIDAsync(int courtID)
        {
            throw new NotImplementedException();
        }

        public List<ICourtBooking> GetCourtBookingsByEventIDAsync(int eventID)
        {
            throw new NotImplementedException();
        }

        public List<ICourtBooking> GetCourtBookingsByOrganiserAsync(int memberID)
        {
            throw new NotImplementedException();
        }

        public List<ICourtBooking> GetCourtBookingsByParticipantsAsync(int memberID)
        {
            throw new NotImplementedException();
        }

        public List<ICourtBooking> GetCourtBookingsByTeamIDAsync(int teamID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCourtBookingAsync(ICourtBooking courtBooking)
        {
            throw new NotImplementedException();
        }
        public bool AddPartisipant(int memberID)
        {
            throw new NotImplementedException();
            // IMember part = new IMember();
            // Participants.Add(part.GetMemberByID(memberID));
        }

        public bool RemovePartisipant(int memberID)
        {
            throw new NotImplementedException();
        }
    }
}
