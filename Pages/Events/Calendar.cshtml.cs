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

        public List<IEvent> Events { get; set; }
        public Dictionary<int, List<IActivity>> EventActivities { get; set; }

        public int CurrentYear { get; set; }
        public int CurrentMonth { get; set; }
        public string MonthName { get; set; } = string.Empty;


        public CalendarModel(IEventDB eventDB, IActivityDB activityDB)
        {
            _eventDB = eventDB;
            _activityDB = activityDB;
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
            await GetAllEventActivities();
        }

        private async Task GetAllEventActivities()
        {
            EventActivities = new();
            foreach (Event ev in Events)
            {
                EventActivities.Add(ev.ID, await _activityDB.GetActivitiesByEventAsync(ev.ID));
            }
        }
        #endregion

        #region Helper Methods
        private string CapitalizeFirstLetter(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1); // Make the first letter capital.
        }

        public List<IActivity> GetEventActivitesByEventID(int eventID)
        {
            if (EventActivities.TryGetValue(eventID, out List<IActivity>? activities))
                return activities;

            return new List<IActivity>();
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

        public async Task OnPostEventRegister(string title, DateTime startDate, DateTime endDate, int currentYear, int currentMonth)
        {
            // Some logic to handle the event register? 

            await LoadCalendar(new DateTime(currentYear, currentMonth, 1));
        }
        #endregion
    }
}
