using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class DeleteEventModel : PageModel
    {
        private IEventDB _eventDB;
        private IActivityDB _activityDB;


        public string? CurrentUser { get; private set; }
        public bool IsAdmin { get; private set; }


        public IEvent? Event { get; private set; }
        public List<IActivity> Activities { get; set; }

        public bool IsDeleted { get; private set; }

        public DeleteEventModel(IEventDB eventDB, IActivityDB activityDB)
        {
            _eventDB = eventDB;
            _activityDB = activityDB;
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

                    return Page();
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }
            return RedirectToPage(@"/Index");
        }
        public async Task OnPostAsync(int eventID)
        {
            try
            {
                IsDeleted = await _eventDB.DeleteEventAsync(eventID);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
