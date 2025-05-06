using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class CreateEventModel : PageModel
    {
        private IEventDB _eventDB;

        public bool IsCreated { get; set; }
        [BindProperty] public Event NewEvent { get; set; }


        public CreateEventModel(IEventDB eventDB)
        { 
            _eventDB = eventDB;
        }
        public void OnGet()
        {
            NewEvent = new Event { Start = DateTime.Today };
        }

        public async Task OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IsCreated = await _eventDB.CreateEventAsync(NewEvent);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
