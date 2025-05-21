using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IEnumerable
{
    public class ActivityDB_IENU : IActivityDB
    {
        public Task<bool> CreateActivityAsync(IActivity activity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteActivityAsync(int eventID, int activityID)
        {
            throw new NotImplementedException();
        }

        public Task<List<IActivity>> GetActivitiesByEventAsync(int eventID)
        {
            throw new NotImplementedException();
        }

        public Task<IActivity?> GetActivityByIDAsync(int eventID, int activityID)
        {
            throw new NotImplementedException();
        }

        public Task<List<IActivity>> GetAllActivitiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateActivityAsync(IActivity activity)
        {
            throw new NotImplementedException();
        }
    }
}
