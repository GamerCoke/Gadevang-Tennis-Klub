using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface ITeamBookingDB
    {
        public Task<List<ITeamBooking>> GetAllTeamBookingAsync();
        public Task<bool> CreateTeamBookingAsync(ITeamBooking teamBooking);
        public Task<bool> DeleteTeamBookingAsync(int teamBookingID);
        public Task<bool> UpdateTeamBookingAsync(ITeamBooking teamBooking);
        public Task<List<IMember>> GetMembersByTeamAsync(int teamId, IMemberDB memberDB);
        public Task<ITeamBooking> GetTeamBookingFromIDAsync(int teamBookingID);
        public Task<int?> GetTeamBookingIDAsync(int teamID, int memberID);
    }
}
