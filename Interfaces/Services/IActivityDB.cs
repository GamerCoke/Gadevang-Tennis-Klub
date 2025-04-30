using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface IActivityDB
    {
        List<IActivity> GetAllActivitiesAsync();
        bool CreateActivityAsync(IActivity activity);
        bool DeleteActivityAsync(int activityID);
        bool UpdateActivityAsync(IActivity activity);
        IActivity GetActivityByIDAsync(int activityID);
        List<IActivity> GetActivityByEventAsync(int eventID);
    }
}
