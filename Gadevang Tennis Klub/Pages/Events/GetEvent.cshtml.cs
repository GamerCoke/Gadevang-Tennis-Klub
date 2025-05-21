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


        public string? CurrentUser { get; private set; }
        public bool IsAdmin { get; private set; }


        public IEvent Event { get; set; }
        public List<IActivity> Activities { get; set; }
        public List<IEventBooking> EventBookings { get; set; }


        public GetEventModel(IEventDB eventDB, IActivityDB activityDB, IEventBookingDB eventBookingDB)
        {
            _eventDB = eventDB;
            _activityDB = activityDB;
            _eventBookingDB = eventBookingDB;
        }

        public async Task<IActionResult> OnGetAsync(int eventID)
        {
            // Validate if user is logged in, and is admin before showing data.         
            CurrentUser = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(CurrentUser))
            {
                return RedirectToPage(@"/User/Login");
            }

            IsAdmin = bool.Parse(CurrentUser.Split('|')[1]);
            if (IsAdmin)
            {
                try
                {
                    Event = await _eventDB.GetEventByIDAsync(eventID);
                    Activities = await _activityDB.GetActivitiesByEventAsync(eventID);
                    EventBookings = await _eventBookingDB.GetEventBookingsByEventIDAsync(eventID);

                    return Page();
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }
            return RedirectToPage(@"/Index");
        }
    }
}
