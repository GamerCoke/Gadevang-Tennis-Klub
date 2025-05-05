using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class GetAllEventsModel : PageModel
    {
        private IEventDB _eventDB;

        //public string? CurrentUser { get; private set; }
        public List<IEvent> Events { get; set; }


        public GetAllEventsModel(IEventDB eventDB)
        {
            _eventDB = eventDB;
        }

        public async Task OnGetAsync()
        {
            try
            {
                //CurrentUser = HttpContext.Session.GetString("UserName");

                Events = await _eventDB.GetAllEventsAsync();
            }
            catch (Exception ex)
            {
                Events = new List<IEvent>();
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}