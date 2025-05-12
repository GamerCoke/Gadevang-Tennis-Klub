using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Globalization;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class CalendarModel : PageModel
    {
        private IEventDB _eventDB;
        private IActivityDB _activityDB;
        private IEventBookingDB _eventBookingDB;

        public int CurrentYear { get; set; }
        public int CurrentMonth { get; set; }
        public string MonthName { get; set; } = string.Empty;


        public List<IEvent> Events { get; set; }


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
            await LoadCalendar(displayDate);
        }

        /// <summary>
        /// Sets the current year and month, updates the month name, and gets event data.
        /// </summary>
        private async Task LoadCalendar(DateTime date)
        {
            CurrentYear = date.Year;
            CurrentMonth = date.Month;
            GetMonthName();
            await GetEventData();
        }

        /// <summary>
        /// Retrieves the local month name and capitalizes its first letter.
        /// </summary>
        private void GetMonthName()
        {
            MonthName = CapitalizeFirstLetter(new CultureInfo("da-DK").DateTimeFormat.GetMonthName(CurrentMonth));
        }

        private async Task GetEventData()
        {
            Events = await _eventDB.GetAllEventsAsync();
        }
        #endregion

        #region Helper Methods
        private string CapitalizeFirstLetter(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1); // Make the first letter capital.
        }
        #endregion

        #region OnPost Methods (Invoked by buttons)
        public async Task OnPostPrevious(int year, int month)
        {
            DateTime currentMonth = new DateTime(year, month, 1);
            DateTime previous = currentMonth.AddMonths(-1);
            await LoadCalendar(previous);
        }

        public async Task OnPostNext(int year, int month)
        {
            DateTime currentMonth = new DateTime(year, month, 1);
            DateTime next = currentMonth.AddMonths(1);
            await LoadCalendar(next);
        }

        public async Task OnPostToday()
        {
            await LoadCalendar(DateTime.Now);
        }

        public async Task<IActionResult> OnPostOpenEventModal(int year, int month, int eventID)
        {
            await LoadCalendar(new DateTime(year, month, 1));

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

        public async Task OnPostEventRegister(int currentYear, int currentMonth, int eventID)
        {
            try
            {
                int memberID = 0; // TEMP

                // Make sure the member isn't already booked for this event before creating a new booking
                CurrentEventBookings = await _eventBookingDB.GetEventBookingsByEventIDAsync(eventID);
                IEventBooking? evBooking = CurrentEventBookings.FirstOrDefault(e => e.MemberID == memberID);
                if (evBooking == null)
                {
                    await _eventBookingDB.CreateEventBookingAsync(new EventBooking(memberID, eventID));
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            await LoadCalendar(new DateTime(currentYear, currentMonth, 1));
        }
        #endregion
    }
}
