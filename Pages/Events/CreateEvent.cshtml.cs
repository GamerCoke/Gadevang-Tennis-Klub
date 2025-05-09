using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class CreateEventModel : PageModel
    {
        private IEventDB _eventDB;
        private IActivityDB _activityDB;

        public bool IsCreated { get; set; }
        [BindProperty] public Event NewEvent { get; set; }
        [BindProperty] public List<Activity> NewActivities { get; set; }


        public CreateEventModel(IEventDB eventDB, IActivityDB activityDB)
        {
            _eventDB = eventDB;
            _activityDB = activityDB;
        }
        public void OnGet()
        {
            NewEvent = new Event { Start = DateTime.Today.AddHours(10), End = new TimeOnly(20, 0) }; // Adds default date and time values to the new event.
            NewActivities = new();
        }

        public void OnPostAddActivity()
        {
            NewActivities.Add(new Activity { Start = NewEvent.Start, End = NewEvent.End } );
            ModelState.Clear(); // Removes validation errors from the event fields
        }

        public void OnPostRemoveActivity(int index)
        {
            NewActivities.RemoveAt(index);
            NewActivities = NewActivities.ToList(); // Rebuild the activities list to make sure all indexes are correct
            ModelState.Clear(); // Removes validation errors from the event fields
        }

        public async Task OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Create the event
                    int? eventId = await _eventDB.CreateEventAsync(NewEvent);
                    if (eventId == null) throw new Exception("Kunne ikke finde id på den oprettede beginvenhed");

                    // Then add the activities to the event
                    if (NewActivities != null && NewActivities.Count > 0)
                    {
                        foreach (Activity activity in NewActivities)
                        {
                            activity.EventID = eventId.Value;
                            await _activityDB.CreateActivityAsync(activity);
                        }
                    }

                    IsCreated = true;
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
