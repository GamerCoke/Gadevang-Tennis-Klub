using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class UpdateEventModel : PageModel
    {
        private IEventDB _eventDB;
        private IActivityDB _activityDB;

        public bool IsUpdated { get; set; }
        [BindProperty] public Event Event { get; set; }
        [BindProperty] public List<Activity> Activities { get; set; }

        [BindProperty] public List<int> DeletedActivities { get; set; }


        public UpdateEventModel(IEventDB eventDB, IActivityDB activityDB)
        {
            _eventDB = eventDB;
            _activityDB = activityDB;
        }

        public async Task OnGet(int eventID)
        {
            try
            {
                Event = (Event)await _eventDB.GetEventByIDAsync(eventID);
                Activities = (await _activityDB.GetActivitiesByEventAsync(eventID)).Cast<Activity>().ToList();

                DeletedActivities = new();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }

        public void OnPostAddActivity()
        {
            Activities.Add(new Activity { Start = Event.Start, End = Event.End }); 
            ModelState.Clear(); // Removes validation errors from the event fields
        }

        public void OnPostRemoveActivity(int index)
        {
            int activityID = Activities[index].ID;
            if (activityID != 0) DeletedActivities.Add(activityID); // If the activity's ID is 0, it is most likely not created in the database yet, so we do not need to add it to the delete from DB list.
            
            Activities.RemoveAt(index);
            Activities = Activities.ToList(); // Rebuild the activities list to make sure all indexes are correct
            ModelState.Clear(); // Removes validation errors from the event fields
        }

        public async Task OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {

                    // Update the event
                    bool isUpdated = await _eventDB.UpdateEventAsync(Event);
                    if (!isUpdated) throw new Exception("Kunne ikke opdatere begivenheden");

                    // Handle deletions
                    // First make sure that all the deleted activities gets removed from the database
                    foreach (int activityID in DeletedActivities)
                    {
                        await _activityDB.DeleteActivityAsync(Event.ID, activityID);
                    }

                    // Handle updates
                    // Retrieve all the activities that remains in the database and update those
                    List<Activity> activitiesInDB = (await _activityDB.GetActivitiesByEventAsync(Event.ID)).Cast<Activity>().ToList();
                    List<Activity> activitiesToUpdate = Activities.Where(a => activitiesInDB.Any(inDB => inDB.ID == a.ID)).ToList();
                    foreach (Activity aToUpdate in activitiesToUpdate)
                    {
                        await _activityDB.UpdateActivityAsync(aToUpdate);
                    }

                    // Handle new activities
                    // Make sure to add all the new activities to the database as the last thing
                    List<Activity> activitiesToCreate = Activities.Where(a => a.ID == 0).ToList(); // If the activity's ID is 0, it is most likely not created in the database yet.
                    foreach (Activity activity in activitiesToCreate)
                    {
                        activity.EventID = Event.ID;
                        await _activityDB.CreateActivityAsync(activity);
                    }

                    // The event (and all of its activities) should now be updated :-)
                    IsUpdated = true;
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
