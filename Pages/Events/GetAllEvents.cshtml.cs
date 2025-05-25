using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Concurrent;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class GetAllEventsModel : PageModel
    {
        private IEventDB _eventDB;
        private IEventBookingDB _eventBookingDB;


        public string? CurrentUser { get; private set; }
        public bool IsAdmin { get; private set; }


        public bool ShowUpcomingEvents { get; set; }


        public List<IEvent> Events { get; set; }
        public ConcurrentDictionary<int, List<IEventBooking>> EventBookingsDict { get; set; } 


        public GetAllEventsModel(IEventDB eventDB, IEventBookingDB eventBookingDB)
        {
            _eventDB = eventDB;
            _eventBookingDB = eventBookingDB;
        }

        public async Task<IActionResult> OnGetAsync(bool showUpcomingEvents = true)
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
                ShowUpcomingEvents = showUpcomingEvents;

                try
                {
                    if (ShowUpcomingEvents)
                    {
                        Events = _eventDB.SortEventsByDate((await _eventDB.GetAllEventsAsync()).Where(e => e.Start > DateTime.Now).ToList());
                    }
                    else Events = _eventDB.SortEventsByDate((await _eventDB.GetAllEventsAsync()).Where(e => e.Start < DateTime.Now).ToList());

                    EventBookingsDict = new();
                    foreach (IEvent ev in Events)
                    {
                        EventBookingsDict.TryAdd(ev.ID, await _eventBookingDB.GetEventBookingsByEventIDAsync(ev.ID));
                    }

                    return Page();
                }
                catch (Exception ex)
                {
                    Events = new List<IEvent>();
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }
            return RedirectToPage(@"/Index");
        }

        public async Task<IActionResult> OnPostShowUpcomingEventsAsync()
        {
            ShowUpcomingEvents = true;

            try
            {
                Events = _eventDB.SortEventsByDate((await _eventDB.GetAllEventsAsync()).Where(e => e.Start > DateTime.Now).ToList());

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

            return Page();
        }

        public async Task<IActionResult> OnPostShowPreviousEventsAsync()
        {
            ShowUpcomingEvents = false;

            try
            {
                Events = _eventDB.SortEventsByDate((await _eventDB.GetAllEventsAsync()).Where(e => e.Start < DateTime.Now).ToList());

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

            return Page();
        }
    }
}