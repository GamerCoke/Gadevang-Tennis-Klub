using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Concurrent;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class GetAllEventsModel : PageModel
    {
        private IEventDB _eventDB;
        private IEventBookingDB _eventBookingDB;

        //public string? CurrentUser { get; private set; }
        public List<IEvent> Events { get; set; }
        public ConcurrentDictionary<int, List<IEventBooking>> EventBookingsDict { get; set; } 


        public GetAllEventsModel(IEventDB eventDB, IEventBookingDB eventBookingDB)
        {
            _eventDB = eventDB;
            _eventBookingDB = eventBookingDB;
        }

        public async Task OnGetAsync()
        {
            try
            {
                //CurrentUser = HttpContext.Session.GetString("UserName");

                Events = _eventDB.SortEventsByDate(await _eventDB.GetAllEventsAsync());

                EventBookingsDict = new();
                foreach (IEvent ev in Events)
                {
                    EventBookingsDict.TryAdd(ev.ID, await _eventBookingDB.GetEventBookingsByEventIDAsync(ev.ID));
                }
            }
            catch (Exception ex)
            {
                Events = new List<IEvent>();
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}