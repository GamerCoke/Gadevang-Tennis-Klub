using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IENumerable
{
    public class ActivityDB_IENU : IActivityDB
    {
        public async Task<bool> CreateActivityAsync(IActivity activity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteActivityAsync(int eventID, int activityID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IActivity>> GetActivitiesByEventAsync(int eventID)
        {
            throw new NotImplementedException();
        }

        public async Task<IActivity?> GetActivityByIDAsync(int eventID, int activityID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IActivity>> GetAllActivitiesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateActivityAsync(IActivity activity)
        {
            throw new NotImplementedException();
        }
    }
}
