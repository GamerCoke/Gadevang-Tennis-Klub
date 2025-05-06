using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class UpdateEventModel : PageModel
    {
        private IEventDB _eventDB;

        public bool IsUpdated { get; set; }
        [BindProperty] public Event Event { get; set; }


        public UpdateEventModel(IEventDB eventDB)
        {
            _eventDB = eventDB;
        }

        public async Task OnGet(int eventID)
        {
            try
            {
                Event = (Event)await _eventDB.GetEventByIDAsync(eventID);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }

        public async Task OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IsUpdated = await _eventDB.UpdateEventAsync(Event);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
