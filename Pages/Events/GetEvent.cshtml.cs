using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class GetEventModel : PageModel
    {
        private IEventDB _eventDB;
        private IActivityDB _activityDB;

        public Event Event { get; set; }
        public List<Activity> Activities { get; set; }


        public GetEventModel(IEventDB eventDB, IActivityDB activityDB)
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
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
