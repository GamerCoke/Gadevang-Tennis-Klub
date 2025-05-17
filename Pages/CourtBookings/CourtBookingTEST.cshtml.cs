using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings
{
    public class CourtBookingTESTModel : PageModel
    {
        ICourtDB _courtDB;
        ICourtBookingDB _courtBookingDB;
        ITeamDB _teamDB;
        IEventDB _eventDB;


        public string? CurrentUser { get; private set; }


        public int HourFrom = 6; // Admin should maybe be able to change this?
        public int HourTo = 23; // Admin should maybe be able to change this?

        public int TimeSlots = 17; // Admin should maybe be able to change this?
        public int HourToBeginFrom = 6; // Admin should maybe be able to change this?


        public List<DateTime> CurrentWeekDays { get; set; }


        public List<string> Areas { get; set; }
        public Dictionary<string, List<ICourt>> CourtsDict { get; set; } // Links a court to an area.


        // Make the current bookings into a dict for a quicker (and more performant) lookup. Combined key with court ID, date & timeslot.
        public Dictionary<(int CourtID, DateOnly Date, int TimeSlot), ICourtBooking> CurrentBookingsDict { get; set; }


        // Area:
        [BindProperty] public int SelectedArea { get; set; }
        public List<SelectListItem> AreaSelectList { get; set; }
        public string CurrentArea { get; set; }


        // Calendar date:
        [BindProperty] public DateTime SelectedDate { get; set; }



        // For the Modal (event popup box)
        public ICourt? CurrentCourt { get; set; }
        public DateTime? CurrentDate { get; set; }
        public int? CurrentTimeSlot { get; set; }




        public CourtBookingTESTModel(ICourtDB courtDB, ICourtBookingDB courtBookingDB, ITeamDB teamDB, IEventDB eventDB)
        {
            _courtDB = courtDB;
            _courtBookingDB = courtBookingDB;
            _teamDB = teamDB;
            _eventDB = eventDB;

            Areas = new List<string>();
            CourtsDict = new Dictionary<string, List<ICourt>>();
            CurrentBookingsDict = new Dictionary<(int CourtID, DateOnly Date, int TimeSlot), ICourtBooking>();
        }

        public async Task OnGetAsync()
        {
            if (SelectedDate == default)
                SelectedDate = DateTime.Today;

            await OnPageReload();
        }

        private async Task OnPageReload()
        {
            await LinkCourtsToAreas();
            await GenerateCurrentBookingsDict();

            // Create dropdown list for areas.
            CreateAreaSelectList();
            CurrentArea = Areas[SelectedArea];

            await GetCurrentWeekDays(SelectedDate);
        }

        private async Task LinkCourtsToAreas()
        {
            List<ICourt> allCourts = await _courtDB.GetAllCourtsAsync();
            foreach (ICourt court in allCourts)
            {
                if (CourtsDict.TryGetValue(court.Type, out List<ICourt>? courts)) // If the area already exists in the dictionary, then add the court to that area.
                {
                    courts.Add(court);
                }
                else // Else add a new area and link the court to that area.
                {
                    CourtsDict.Add(court.Type, new List<ICourt> { court });
                    Areas.Add(court.Type);
                }
            }
        }
        private async Task GenerateCurrentBookingsDict()
        {
            List<ICourtBooking> currentCourtBookings = await _courtBookingDB.GetAllCourtBookingsAsync();
            foreach (ICourtBooking courtBooking in currentCourtBookings)
            {
                CurrentBookingsDict.Add((courtBooking.Court_ID, courtBooking.Date, courtBooking.Timeslot), courtBooking);
            }
        }


        private async Task GetCurrentWeekDays(DateTime day)
        {
            CurrentWeekDays = new List<DateTime>(); // This list is going to contain the 7 weekdays that we currently see on the screen.

            // Calculate the most recent Monday (or today if it's Monday) so we can start the week from there.
            int daysSinceMonday = (7 + (int)day.DayOfWeek - (int)DayOfWeek.Monday) % 7;
            DateTime monday = day.AddDays(-daysSinceMonday);

            for (int d = 0; d < 7; d++) // Then add exact date for each day in the week.
            {
                DateTime weekDayDateTime = monday.AddDays(d).Date + day.TimeOfDay; // Store the date of each of the week days.
                CurrentWeekDays.Add(weekDayDateTime); // weekDay);
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

        public bool CheckIsAvailable(ICourt court, DateTime date, int timeSlot)
        {
            DateTime slotDateTime = date.AddHours(timeSlot+HourToBeginFrom); // Combine date and time
            DateTime now = DateTime.Now;

            bool isWithin14Days = slotDateTime >= now && slotDateTime <= now.AddDays(14);

            return isWithin14Days;
        }
        public bool CheckIsBooked(ICourt court, DateTime date, int timeSlot)
        {
            var lookupKey = (court.ID, DateOnly.FromDateTime(date), timeSlot);

            return CurrentBookingsDict.ContainsKey(lookupKey);
        }
        public bool CheckIsBookedByMe(ICourt court, DateTime date, int timeSlot)
        {
            CurrentUser = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(CurrentUser)) return false; // return early if a user could not be found.

            int memberID = int.Parse(CurrentUser.Split('|')[0]); // Get the member id

            var lookupKey = (court.ID, DateOnly.FromDateTime(date), timeSlot);
            return CurrentBookingsDict.TryGetValue(lookupKey, out ICourtBooking? courtBooking) && courtBooking?.Member_ID == memberID;
        }

        public async Task<string> GetHoverTitle(ICourt court, DateTime date, int timeSlot, bool isAvailable, bool isBooked)
        {
            string hoverTitle = !isAvailable ? "Kan ikke bookes" : (isBooked ? await GetIsBookedString(court, date, timeSlot) : GetIsAvailableString(court, timeSlot));

            return hoverTitle;
        }

        private async Task<string> GetIsBookedString(ICourt court, DateTime date, int timeSlot)
        {
            var lookupKey = (court.ID, DateOnly.FromDateTime(date), timeSlot);
            if (!CurrentBookingsDict.TryGetValue(lookupKey, out ICourtBooking? courtBooking)) // return early if the booking could not be found.
                return "";

            if (courtBooking.Team_ID is int bookingTeamID)
            {
                ITeam? team = await _teamDB.GetTeamByIDAsync(bookingTeamID);
                return $"Holdtræning: {team?.Name}";
            }

            if (courtBooking.Member_ID is int bookingMemberID)
            {
                string? currentUser = HttpContext.Session.GetString("User");
                if (!string.IsNullOrEmpty(currentUser))
                {
                    if (int.TryParse(currentUser.Split('|')[0], out int memberID))
                    {
                        if (bookingMemberID == memberID)
                        {
                            List<IMember> participants = courtBooking.Participants.ToList();
                            string participantsString = "";
                            foreach (IMember participant in participants)
                            {
                                participantsString += $"\n{ participant.Name}";
                            }
                            return $"Din booking: Bane {court.ID} {court.Type} kl. {timeSlot + HourToBeginFrom}:00" +
                                    $"\nBooket med: {participantsString}";
                        }
                    }
                }
                return "Medlemsbooking";
            }

            if (courtBooking.Event_ID is int bookingEventID)
            {
                IEvent? ev = await _eventDB.GetEventByIDAsync(bookingEventID);
                return $"Begivenhed: {ev?.Title}";
            }

            return "";
        }
        private string GetIsAvailableString(ICourt court, int timeSlot)
        {
            return $"Bane {court.ID} {court.Type} kl. {timeSlot+HourToBeginFrom}:00. Ledig: klik for at booke";
        }

        public string GetIsBookedIcon(ICourt court, DateTime date, int timeSlot)
        {
            var lookupKey = (court.ID, DateOnly.FromDateTime(date), timeSlot);
            if (!CurrentBookingsDict.TryGetValue(lookupKey, out ICourtBooking? courtBooking)) // return early if the booking could not be found.
                return "";

            if (courtBooking.Team_ID is int bookingTeamID)
            {
                return $"fa-solid fa-clock";
            }

            if (courtBooking.Member_ID is int bookingMemberID)
            {
                return "fa-solid fa-user";
            }

            if (courtBooking.Event_ID is int bookingEventID)
            {
                return $"fa-solid fa-calendar";
            }

            return "";
        }

        public async Task OnPostNewAreaChosen()
        {
            await OnPageReload();
        }

        public async Task OnPostNewDateChosen()
        {
            await OnPageReload();
        }

        public async Task<IActionResult> OnPostOpenBookCourtModal(int courtID, DateTime date, int timeSlot)
        {
            CurrentUser = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(CurrentUser))
            {
                return RedirectToPage(@"/User/Login");
            }

            try
            {
                CurrentCourt = await _courtDB.GetCourtByIDAsync(courtID);
                CurrentDate = date;
                CurrentTimeSlot = timeSlot;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            //SelectedDate = date;
            await OnPageReload();
            return Page();
        }

        public async Task<IActionResult> OnPostBookCourt(int courtID, DateTime date, int timeSlot)
        {
            CurrentUser = HttpContext.Session.GetString("User");
            int memberID = int.Parse(CurrentUser.Split('|')[0]); // Get the member id

            try
            { 
                ICourtBooking newCourtBooking = new CourtBooking(0, courtID, DateOnly.FromDateTime(date), timeSlot, null, memberID, null);
                await _courtBookingDB.CreateCourtBookingAsync(newCourtBooking);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            //SelectedDate = date;
            await OnPageReload();
            return Page();
        }

        public async Task OnPostPrevious()
        {
            SelectedDate = SelectedDate.AddDays(-7);
            await OnPageReload();
        }

        public async Task OnPostNext()
        {
            SelectedDate = SelectedDate.AddDays(7);
            await OnPageReload();
        }

        public async Task OnPostToday()
        {
            SelectedDate = DateTime.Today;
            await OnPageReload();
        }
    }
}
