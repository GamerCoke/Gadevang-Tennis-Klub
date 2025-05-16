using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings
{
    public class CourtBookingTESTModel : PageModel
    {
        ICourtDB _courtDB;
        ICourtBookingDB _courtBookingDB;

        public int Hour_From = 6; // Admin should maybe be able to change this.
        public int Hour_To = 23; // Admin should maybe be able to change this.

        // Data lists from BookingService:
        public Dictionary<(int CourtNo, DateTime Date), ICourtBookingDB> CurrentBookings { get; set; }
        //public Dictionary<(int CourtNo, DateTime Date), Booking> UnavailableBookings { get; set; }
        public List<string> Areas { get; set; }
        public Dictionary<string, List<ICourt>> CourtsDict { get; set; }

        // Booking table data:
        public List<DateTime> CurrentWeekDays { get; set; }

        // Area:
        [BindProperty] public int SelectedArea { get; set; }
        public List<SelectListItem> AreaSelectList { get; set; }
        public string CurrentArea { get; set; }

        // Calendar date:
        [BindProperty] public DateTime SelectedDate { get; set; }

        public CourtBookingTESTModel(ICourtDB courtDB, ICourtBookingDB courtBookingDB)
        {
            _courtDB = courtDB;
            _courtBookingDB = courtBookingDB;
        }

        public async Task OnGetAsync()
        {
            if (SelectedDate == default)
                SelectedDate = DateTime.Today;

            await OnPageReload();
        }

        private async Task OnPageReload()
        {
            // Get data from BookingService.
            //            Areas = _bookingService.GetAllAreas();
            //            CurrentBookings = _bookingService.GetAllBookings();
            //           UnavailableBookings = _bookingService.GetAllUnavailableBookings();
            Areas = new List<string>();
            CourtsDict = new Dictionary<string, List<ICourt>>();

            List<ICourt> allCourts = await _courtDB.GetAllCourtsAsync();
            foreach (ICourt court in allCourts)
            {
                if (CourtsDict.TryGetValue(court.Type, out List<ICourt>? courts))
                {
                    courts.Add(court);
                }
                else
                {
                    CourtsDict.Add(court.Type, new List<ICourt>{ court });
                    Areas.Add(court.Type);
                }
            }
            //CurrentBookings = await _courtBookingDB.GetAllCourtBookingsAsync();

            #region TEMP
/*            Areas = new List<string>
            {
                "Udendørs tennisbaner",
                "Indendørs tennisbaner",
                "Udendørs padelbaner"
            };*/

            #endregion

            // Create dropdown list for areas.
            CreateAreaSelectList();
            CurrentArea = Areas[SelectedArea];

            // Generate the actual booking table data.
            await GenerateBookingTableData(SelectedDate);
        }

        private async Task GenerateBookingTableData(DateTime day)
        {
            await FillCurrentWeekDaysList(day);
        }

        private async Task FillCurrentWeekDaysList(DateTime day)
        {
            CurrentWeekDays = new List<DateTime>();

            // Calculate the most recent Monday (or today if it's Monday)
            int daysSinceMonday = (7 + (int)day.DayOfWeek - (int)DayOfWeek.Monday) % 7;
            DateTime monday = day.AddDays(-daysSinceMonday);

            for (int d = 0; d < 7; d++) // For each day in the week.
            {
                DateTime weekDayDate = monday.AddDays(d); // Store the date of each of the week days.
                DateTime weekDay = weekDayDate;//new DaySlot { Date = weekDayDate, CourtSlots = new List<CourtSlot>() }; // Create a new day slot.

                AddTimeSlots(weekDayDate, weekDay);

                CurrentWeekDays.Add(weekDay);
            }
        }

        private void AddTimeSlots(DateTime weekDayDate, DateTime weekDay)
        {
            for (int hour = Hour_From; hour <= Hour_To; hour++) // For each available time slot.
            {
                AddCourtSlots(weekDayDate, weekDay, hour);
            }
        }

        private void AddCourtSlots(DateTime weekDayDate, DateTime weekDay, int hour)
        {
            for (int courtIndex = 0; courtIndex < CourtsDict.Count; courtIndex++) // Add the specified amount of courts for each time slot.
            {
            // Temp var's to use them when creating the new CourtSlot.
            //Court court = CurrentArea.Courts[courtIndex];
            //DateTime date = new DateTime(weekDayDate.Year, weekDayDate.Month, weekDayDate.Day, hour, 0, 0);
            //var lookupKey = (court.CourtNo, date);

/*            weekDay.CourtSlots.Add(new CourtSlot
            {
                Court = court,
                Date = date,  // Add the time slot to each court slot.
                IsAvailable = true, //!UnavailableBookings.ContainsKey(lookupKey) && date >= DateTime.Now && date <= DateTime.Now.AddDays(14),  // Logic to check if the court is available.
                IsBooked = false // CurrentBookings.ContainsKey(lookupKey) // Logic to check if the court is booked
            });*/
            }
        }

        private void CreateAreaSelectList()
        {
            AreaSelectList = new List<SelectListItem>();
            int i = 0;
            foreach (string area in Areas)
            {
                SelectListItem selectListItem = new SelectListItem($"{area}", i.ToString());
                AreaSelectList.Add(selectListItem);
                i += 1;
            }
        }

        public void OnPostNewAreaChosen()
        {
            OnPageReload();
        }
    }
}
