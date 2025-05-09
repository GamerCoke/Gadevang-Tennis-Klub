using Gadevang_Tennis_Klub.Interfaces.Models;
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

        public IEvent Event { get; set; }
        public List<IActivity> Activities { get; set; }


        public GetEventModel(IEventDB eventDB, IActivityDB activityDB)
        {
            _eventDB = eventDB;
            _activityDB = activityDB;
        }

        public async Task OnGet(int eventID)
        {
            try
            {
                Event = await _eventDB.GetEventByIDAsync(eventID);
                Activities = await _activityDB.GetActivitiesByEventAsync(eventID);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
