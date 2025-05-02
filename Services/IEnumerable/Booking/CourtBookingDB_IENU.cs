using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IENumerable.Booking
{
    public class CourtBookingDB_IENU : ICourtBookingDB
    {
        public Task<bool> CreateCourtBookingAsync(ICourtBooking courtBooking)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCourtBookingAsync(int courtBookingID)
        {
            throw new NotImplementedException();
        }

        public Task<List<ICourtBooking>> GetAllCourtBookingsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICourtBooking> GetCourtBookingByIDAsync(int courtBookingID)
        {
            throw new NotImplementedException();
        }

        public Task<List<ICourtBooking>> GetCourtBookingsByCourtIDAsync(int courtID)
        {
            throw new NotImplementedException();
        }

        public Task<List<ICourtBooking>> GetCourtBookingsByEventIDAsync(int eventID)
        {
            throw new NotImplementedException();
        }

        public Task<List<ICourtBooking>> GetCourtBookingsByOrganiserAsync(int memberID)
        {
            throw new NotImplementedException();
        }

        public Task<List<ICourtBooking>> GetCourtBookingsByParticipantsAsync(int memberID)
        {
            throw new NotImplementedException();
        }

        public Task<List<ICourtBooking>> GetCourtBookingsByTeamIDAsync(int teamID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCourtBookingAsync(ICourtBooking courtBooking)
        {
            throw new NotImplementedException();
        }
        public Task<bool> AddPartisipantAsync(int bookingID, int memberID)
        {
            throw new NotImplementedException();
        }
        public Task<bool> RemovePartisipantAsync(int bookingID, int memberID)
        {
            throw new NotImplementedException();
        }
    }
}
