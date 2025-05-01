using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.SQL
{
    public class ActivityDB : IActivityDB
    {
        public bool CreateActivityAsync(IActivity activity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteActivityAsync(int activityID)
        {
            throw new NotImplementedException();
        }

        public List<IActivity> GetActivityByEventAsync(int eventID)
        {
            throw new NotImplementedException();
        }

        public IActivity GetActivityByIDAsync(int activityID)
        {
            throw new NotImplementedException();
        }

        public List<IActivity> GetAllActivitiesAsync()
        {
            throw new NotImplementedException();
        }

        public bool UpdateActivityAsync(IActivity activity)
        {
            throw new NotImplementedException();
        }
    }
}
