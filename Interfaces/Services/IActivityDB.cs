using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface IActivityDB
    {
        /// <summary>
        /// Gets all activities from the database
        /// </summary>
        /// <returns>List of activities</returns>
        Task<List<IActivity>> GetAllActivitiesAsync();

        /// <summary>
        /// Inserts a new activity into the database
        /// </summary>
        /// <param name="activity">The activity to be inserted</param>
        /// <returns>True if the insert was successfull, otherwise false</returns>
        Task<bool> CreateActivityAsync(IActivity activity);

        /// <summary>
        /// Deletes an activity from the database
        /// </summary>
        /// <param name="eventID">The ID of the event linked to this activity</param>
        /// <param name="activityID">The ID of the activity to delete</param>
        /// <returns>True if the insert was successfull, otherwise false</returns>
        Task<bool> DeleteActivityAsync(int eventID, int activityID);

        /// <summary>
        /// Updates an activity from the database
        /// </summary>
        /// <param name="activity">The activity containing the new data</param>
        /// <returns>True if the insert was successfull, otherwise false</returns>
        Task<bool> UpdateActivityAsync(IActivity activity);

        /// <summary>
        /// Gets a specific activity from the database
        /// </summary>
        /// <param name="eventID">The ID of the event linked to this activity</param>
        /// <param name="activityID">The ID of the activity to retrieve</param>
        /// <returns>The activity found or null if the hotel does not exist</returns>
        Task<IActivity?> GetActivityByIDAsync(int eventID, int activityID);

        /// <summary>
        /// Gets all activites for a specific event from the database
        /// </summary>
        /// <param name="eventID">The id of the event to retrieve activities from</param>
        /// <returns>List of activities</returns>
        Task<List<IActivity>> GetActivitiesByEventAsync(int eventID);
    }
}