using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IEnumerable.Booking
{
    public class TeamBookingDB_IENU : ITeamBookingDB
    {
        public Task<List<IMember>> GetMembersByTeamAsync(int teamId, IMemberDB memberDB)
        {
            throw new NotImplementedException();
        }

        public Task<int?> GetTeamBookingIDAsync(int teamID, int memberID)
        {
            throw new NotImplementedException();
        }

        Task<bool> ITeamBookingDB.CreateTeamBookingAsync(ITeamBooking teamBooking)
        {
            throw new NotImplementedException();
        }

        Task<bool> ITeamBookingDB.DeleteTeamBookingAsync(int teamBookingID)
        {
            throw new NotImplementedException();
        }

        Task<List<ITeamBooking>> ITeamBookingDB.GetAllTeamBookingAsync()
        {
            throw new NotImplementedException();
        }

        Task<ITeamBooking> ITeamBookingDB.GetTeamBookingFromIDAsync(int teamBookingID)
        {
            throw new NotImplementedException();
        }

        Task<bool> ITeamBookingDB.UpdateTeamBookingAsync(ITeamBooking teamBooking)
        {
            throw new NotImplementedException();
        }
    }
}
