using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using System.Globalization;

namespace Gadevang_Tennis_Klub.Pages.Events
{
    public class CalendarModel : PageModel
    {
        private IEventDB _eventDB;

        public List<Event> Events { get; set; }

        public int CurrentYear { get; set; }
        public int CurrentMonth { get; set; }
        public string MonthName { get; set; } = string.Empty;


        public CalendarModel(IEventDB eventDB)
        {
            _eventDB = eventDB;
        }

        public void OnGet(int? year, int? month)
        {
            DateTime displayDate = (year.HasValue && month.HasValue) ? new DateTime(year.Value, month.Value, 1) : DateTime.Now;
            LoadCalendar(displayDate);
        }

        /// <summary>
        /// Sets the current year and month, updates the month name, and generates mock event data.
        /// </summary>
        private void LoadCalendar(DateTime date)
        {
            CurrentYear = date.Year;
            CurrentMonth = date.Month;
            GetMonthName();
            GetEventData();
        }

        private async Task GetEventData()
        {
            Events = (await _eventDB.GetAllEventsAsync()).Cast<Event>().ToList();
        }

        /// <summary>
        /// Retrieves the local month name and capitalizes its first letter.
        /// </summary>
        private void GetMonthName()
        {
            MonthName = CapitalizeFirstLetter(new CultureInfo("da-DK").DateTimeFormat.GetMonthName(CurrentMonth));
        }
        private string CapitalizeFirstLetter(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1); // Make the first letter capital.
        }

        public void OnPostPrevious(int year, int month)
        {
            DateTime currentMonth = new DateTime(year, month, 1);
            DateTime previous = currentMonth.AddMonths(-1);
            LoadCalendar(previous);
        }

        public void OnPostNext(int year, int month)
        {
            DateTime currentMonth = new DateTime(year, month, 1);
            DateTime next = currentMonth.AddMonths(1);
            LoadCalendar(next);
        }

        public void OnPostToday()
        {
            LoadCalendar(DateTime.Now);
        }

        public void OnPostEventRegister(string title, DateTime startDate, DateTime endDate, int currentYear, int currentMonth)
        {
            // Some logic to handle the event register? 

            LoadCalendar(new DateTime(currentYear, currentMonth, 1));
        }
    }
}
