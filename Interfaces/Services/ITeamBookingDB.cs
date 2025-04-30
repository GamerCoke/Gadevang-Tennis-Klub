using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface ITeamBookingDB
    {
        public List<ITeamBooking> GetAllTeamBookingAsync();
        public bool CreateTeamBookingAsync(ITeamBooking teamBooking);
        public bool DeleteTeamBookingAsync(int teamBookingID);
        public bool UpdateTeamBookingAsync(ITeamBooking teamBooking);
        public ITeamBooking GetTeamBookingFromIDAsync(int teamBookingID);
    }
}
