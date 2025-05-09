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

        private List<IEvent> SortByDate(List<IEvent> listToSort)
        {
            List<IEvent> sortedList = listToSort;
            sortedList.Sort((d1, d2) => d1.Start.CompareTo(d2.Start));

            return sortedList;
        }

        public async Task OnGetAsync()
        {
            try
            {
                //CurrentUser = HttpContext.Session.GetString("UserName");

                Events = await _eventDB.GetAllEventsAsync();
                SortByDate(Events);
            }
            catch (Exception ex)
            {
                Events = new List<IEvent>();
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}