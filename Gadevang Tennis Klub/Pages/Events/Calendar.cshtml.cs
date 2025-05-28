using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class CalendarModel : PageModel
    {
        private IEventDB _eventDB;
        private IActivityDB _activityDB;
        private IEventBookingDB _eventBookingDB;


        public string? CurrentUser { get; private set; }


        // For the message popup when successfully registering or unregistering
        public string MessageSuccess { get; set; }
        public string MessageDanger { get; set; }


        public int CurrentYear { get; set; }
        public int CurrentMonth { get; set; }
        public string MonthName { get; set; } = string.Empty;


        public List<IEvent> Events { get; set; }


        // For the Modal (event popup box)
        public IEvent? CurrentEvent { get; set; }
        public List<IActivity>? CurrentEventActivities { get; set; }
        public List<IEventBooking>? CurrentEventBookings { get; set; }


        public CalendarModel(IEventDB eventDB, IActivityDB activityDB, IEventBookingDB eventBookingDB)
        {
            _eventDB = eventDB;
            _activityDB = activityDB;
            _eventBookingDB = eventBookingDB;
        }

        #region Initializement Methods
        public async Task OnGetAsync(int? year, int? month)
        {
            DateTime displayDate = (year.HasValue && month.HasValue) ? new DateTime(year.Value, month.Value, 1) : DateTime.Now;
            await LoadCalendarAsync(displayDate);
        }

        /// <summary>
        /// Sets the current year and month, updates the month name, and gets event data.
        /// </summary>
        private async Task LoadCalendarAsync(DateTime date)
        {
            CurrentUser = HttpContext.Session.GetString("User");

            CurrentYear = date.Year;
            CurrentMonth = date.Month;
            GetMonthName();
            await GetEventDataAsync();
        }

        /// <summary>
        /// Retrieves the local month name and capitalizes its first letter.
        /// </summary>
        private void GetMonthName()
        {
            MonthName = CapitalizeFirstLetter(new CultureInfo("da-DK").DateTimeFormat.GetMonthName(CurrentMonth));
        }

        private async Task GetEventDataAsync()
        {
            Events = await _eventDB.GetAllEventsAsync();
        }
        #endregion

        #region Helper Methods
        private string CapitalizeFirstLetter(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1); // Make the first letter capital.
        }
        public async Task<IEventBooking?> GetMemberBookingAsync(int eventID, int memberID)
        {
            CurrentEventBookings = await _eventBookingDB.GetEventBookingsByEventIDAsync(eventID);
            return CurrentEventBookings.FirstOrDefault(e => e.MemberID == memberID);
        }
        #endregion

        #region OnPost Methods (Invoked by buttons)
        public async Task OnPostPreviousAsync(int year, int month)
        {
            DateTime currentMonth = new DateTime(year, month, 1);
            DateTime previous = currentMonth.AddMonths(-1);
            await LoadCalendarAsync(previous);
        }

        public async Task OnPostNextAsync(int year, int month)
        {
            DateTime currentMonth = new DateTime(year, month, 1);
            DateTime next = currentMonth.AddMonths(1);
            await LoadCalendarAsync(next);
        }

        public async Task OnPostTodayAsync()
        {
            await LoadCalendarAsync(DateTime.Now);
        }

        public async Task<IActionResult> OnPostOpenEventModalAsync(int year, int month, int eventID)
        {
            await LoadCalendarAsync(new DateTime(year, month, 1));

            try
            {
                IEvent ev = Events.First(e => e.ID == eventID); // Throws if no event could be found

                CurrentEvent = ev;
                CurrentEventActivities = await _activityDB.GetActivitiesByEventAsync(eventID);
                CurrentEventBookings = await _eventBookingDB.GetEventBookingsByEventIDAsync(eventID);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            return Page(); // Reloads the same page, now with CurrentEvent & CurrentEventActivities populated
        }

        public async Task<IActionResult> OnPostEventRegisterAsync(int currentYear, int currentMonth, int eventID)
        {
            if (!int.TryParse(HttpContext.Session.GetString("User")?.Split('|')[0], out int memberID))
            {
                return RedirectToPage(@"/User/Login");
            }

            try
            {
                IEvent ev = await _eventDB.GetEventByIDAsync(eventID);
                List<IEventBooking> evBookings = await _eventBookingDB.GetEventBookingsByEventIDAsync(eventID);

                // Make sure the event isn't already fully booked
                if (ev.Capacity > evBookings.Count)
                {
                    // Make sure the member isn't already booked for this event before creating a new booking
                    if (await GetMemberBookingAsync(eventID, memberID) == null)
                    {
                        await _eventBookingDB.CreateEventBookingAsync(new EventBooking(memberID, eventID));

                        MessageSuccess = $"Du er nu tilmeldt begivenheden: {ev.Title}";
                    }
                }
                else MessageDanger = $"Der er ikke flere pladser tilbage til begivenheden: {ev.Title}";
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            await LoadCalendarAsync(new DateTime(currentYear, currentMonth, 1)); 
            return Page();
        }

        public async Task<IActionResult> OnPostEventUnregisterAsync(int currentYear, int currentMonth, int eventID)
        {
            if (int.TryParse(HttpContext.Session.GetString("User")?.Split('|')[0], out int memberID))
            {
                try
                {
                    // Make sure the member is actually booked for this event before deleting the booking
                    IEventBooking? evBooking = await GetMemberBookingAsync(eventID, memberID);
                    if (evBooking != null)
                    {
                        await _eventBookingDB.DeleteEventBookingAsync(evBooking.ID);

                        IEvent ev = await _eventDB.GetEventByIDAsync(eventID);
                        MessageDanger = $"Du er nu afmeldt begivenheden: {ev.Title}";
                    }
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }

            await LoadCalendarAsync(new DateTime(currentYear, currentMonth, 1));
            return Page();
        }
        #endregion
    }
}
