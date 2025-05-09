using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface IEventDB
    {
        /// <summary>
        /// Gets all events from the database
        /// </summary>
        /// <returns>List of events</returns>
        Task<List<IEvent>> GetAllEventsAsync();

        /// <summary>
        /// Inserts a new event into the database
        /// </summary>
        /// <param name="ev">The event to be inserted</param>
        /// <returns>True if the insert was successfull, otherwise false</returns>
        Task<int?> CreateEventAsync(IEvent ev);

        /// <summary>
        /// Deletes an event from the database
        /// </summary>
        /// <param name="eventId">The ID of the event to delete</param>
        /// <returns>True if the insert was successfull, otherwise false</returns>
        Task<bool> DeleteEventAsync(int eventId);

        /// <summary>
        /// Updates an event from the database
        /// </summary>
        /// <param name="ev">The event containing the new data</param>
        /// <returns>True if the insert was successfull, otherwise false</returns>
        Task<bool> UpdateEventAsync(IEvent ev);

        /// <summary>
        /// Gets a specific event from the database
        /// </summary>
        /// <param name="eventId">The ID of the event to retrieve</param>
        /// <returns>The event found or null if the hotel does not exist</returns>
        Task<IEvent?> GetEventByIDAsync(int eventId);

        /// <summary>
        /// Gets all events by a specific date from the database
        /// </summary>
        /// <param name="date">The date of the events to retrieve</param>
        /// <returns>List of events</returns>
        Task<List<IEvent>> GetEventsByDateAsync(DateTime date);

        /// <summary>
        /// Gets the capacity of a specific event
        /// </summary>
        /// <param name="eventID">The ID of the event to retrieve capacity from</param>
        /// <returns>The max capacity of the event</returns>
        Task<int?> GetEventCapacityAsync(int eventID);
    }
}
