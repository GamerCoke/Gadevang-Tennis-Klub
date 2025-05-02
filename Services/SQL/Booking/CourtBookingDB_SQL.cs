using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.SQL.Booking
{
    public class CourtBookingDB_SQL : ICourtBookingDB
    {

        public async Task<bool> CreateCourtBookingAsync(ICourtBooking courtBooking)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCourtBookingAsync(ICourtBooking courtBooking)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteCourtBookingAsync(int courtBookingID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ICourtBooking>> GetAllCourtBookingsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ICourtBooking> GetCourtBookingByIDAsync(int courtBookingID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ICourtBooking>> GetCourtBookingsByCourtIDAsync(int courtID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ICourtBooking>> GetCourtBookingsByEventIDAsync(int eventID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ICourtBooking>> GetCourtBookingsByTeamIDAsync(int teamID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ICourtBooking>> GetCourtBookingsByOrganiserAsync(int memberID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ICourtBooking>> GetCourtBookingsByParticipantsAsync(int memberID)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddPartisipant(int memberID)
        {
            throw new NotImplementedException();
            // IMember part = new IMember();
            // Participants.Add(part.GetMemberByID(memberID));
        }

        public async Task<bool> RemovePartisipant(int memberID)
        {
            throw new NotImplementedException();

        }
    }
}
