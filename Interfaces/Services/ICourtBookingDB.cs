using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface ICourtBookingDB
    {
        public Task<bool> CreateCourtBookingAsync(ICourtBooking courtBooking);
        public Task<bool> UpdateCourtBookingAsync(ICourtBooking courtBooking);
        public Task<bool> DeleteCourtBookingAsync(int courtBookingID);
        public Task<List<ICourtBooking>> GetAllCourtBookingsAsync();
        public Task<ICourtBooking> GetCourtBookingByIDAsync(int courtBookingID);
        public Task<List<ICourtBooking>> GetCourtBookingsByCourtIDAsync(int courtID);
        public Task<List<ICourtBooking>> GetCourtBookingsByEventIDAsync(int eventID);
        public Task<List<ICourtBooking>> GetCourtBookingsByTeamIDAsync(int teamID);
        public Task<List<ICourtBooking>> GetCourtBookingsByOrganiserAsync(int memberID);
        public Task<List<ICourtBooking>> GetCourtBookingsByParticipantsAsync(int memberID);
        public Task<bool> AddPartisipant(int memberID);
        public Task<bool> RemovePartisipant(int memberID);
    }
}
