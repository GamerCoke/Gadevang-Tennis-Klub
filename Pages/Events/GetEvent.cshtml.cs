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
        private IEventBookingDB _eventBookingDB;

        public IEvent Event { get; set; }
        public List<IActivity> Activities { get; set; }
        public List<IEventBooking> EventBookings { get; set; }


        public GetEventModel(IEventDB eventDB, IActivityDB activityDB, IEventBookingDB eventBookingDB)
        {
            _eventDB = eventDB;
            _activityDB = activityDB;
            _eventBookingDB = eventBookingDB;
        }

        public async Task OnGet(int eventID)
        {
            try
            {
                Event = await _eventDB.GetEventByIDAsync(eventID);
                Activities = await _activityDB.GetActivitiesByEventAsync(eventID);
                EventBookings = await _eventBookingDB.GetEventBookingsByEventIDAsync(eventID);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
