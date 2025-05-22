using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings
{
    public class CourtBookingModel : PageModel
    {
        ICourtDB _courtDB;
        ITeamDB _teamDB;
        IEventDB _eventDB;

        public ICourtBookingDB CourtBookingDB { get; private set; }

        public string? CurrentUser { get; private set; }


        // For the message popup when successfully booking or removing bookings
        public string MessageSuccess { get; set; }
        public string MessageDanger { get; set; }


        public int MaxBookings { get; } = 4; // Admin should maybe be able to change this?

        public int TimeSlots { get; } = 15; // Admin should maybe be able to change this?
        public int HourToBeginFrom { get; } = 6; // Admin should maybe be able to change this?


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
        public bool IsBooked { get; set; }



        public CourtBookingModel(ICourtDB courtDB, ICourtBookingDB courtBookingDB, ITeamDB teamDB, IEventDB eventDB)
        {
            _courtDB = courtDB;
            CourtBookingDB = courtBookingDB;
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
            List<ICourtBooking> currentCourtBookings = await CourtBookingDB.GetAllCourtBookingsAsync();
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

        public async Task<int> GetMyCurrentBookingsCount(int memberID)
        {
            List<ICourtBooking> participatingCourtBookings = await CourtBookingDB.GetCourtBookingsByParticipantsAsync(memberID);

            int count = 0;

            foreach (ICourtBooking courtBooking in participatingCourtBookings)
            {
                // Convert DateOnly to DateTime and add Timeslot hours
                DateTime bookingDateTime = courtBooking.Date.ToDateTime(TimeOnly.MinValue).AddHours(courtBooking.Timeslot);

                if (CheckIsWithin14Days(bookingDateTime)) count++;
            }

            return count;
        }


        public bool CheckIsWithin14Days(DateTime dateTime)
        {
            DateTime now = DateTime.Now;

            bool isWithin14Days = dateTime >= now && dateTime <= now.AddDays(14);

            return isWithin14Days;
        }

        public bool CheckIsAvailable(DateTime date, int timeSlot)
        {
            return CheckIsWithin14Days(date.AddHours(timeSlot + HourToBeginFrom)); // Combine date and time
        }

        public bool CheckIsBooked(ICourt court, DateTime date, int timeSlot)
        {
            var lookupKey = (court.ID, DateOnly.FromDateTime(date), timeSlot);
            return CurrentBookingsDict.ContainsKey(lookupKey);
        }

        public bool CheckIsBookedByMe(ICourt court, DateTime date, int timeSlot)
        {
            if (!int.TryParse(HttpContext.Session.GetString("User")?.Split('|')[0], out int memberID)) return false; // return early if a user could not be found.

            var lookupKey = (court.ID, DateOnly.FromDateTime(date), timeSlot);
            return CurrentBookingsDict.TryGetValue(lookupKey, out ICourtBooking? courtBooking) && courtBooking?.Member_ID == memberID;
        }

        public async Task<string> GetHoverTitle(ICourt court, DateTime date, int timeSlot, bool isAvailable, bool isBooked)
        {
            return !isAvailable ? "Kan ikke bookes" : (isBooked ? await GetIsBookedString(court, date, timeSlot) : GetIsAvailableString(court, timeSlot));
        }

        private async Task<string> GetIsBookedString(ICourt court, DateTime date, int timeSlot)
        {
            var lookupKey = (court.ID, DateOnly.FromDateTime(date), timeSlot);
            if (!CurrentBookingsDict.TryGetValue(lookupKey, out ICourtBooking? courtBooking)) // return early if the booking could not be found.
                return "";

            // Check if it's a team booking
            if (courtBooking.Team_ID is int bookingTeamID)
            {
                ITeam? team = await _teamDB.GetTeamByIDAsync(bookingTeamID);
                return $"Holdtræning: {team?.Name}";
            }

            // Check if it's a member booking
            if (courtBooking.Member_ID is int bookingMemberID)
            {
                if (int.TryParse(HttpContext.Session.GetString("User")?.Split('|')[0], out int memberID) && 
                    bookingMemberID == memberID)
                {
                    List<IMember> participants = courtBooking.Participants.ToList();
                    string participantNames = participants.Count == 0 ? "boldmaskinen" : $"\n{string.Join("\n", participants.Select(p => p.Name))}";

                    return $"Din booking: Bane {court.ID} {court.Type} kl. {timeSlot + HourToBeginFrom}:00" +
                            $"\nBooket med: {participantNames}";
                }
                return "Medlemsbooking";
            }

            // Check if it's an event booking
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

        public async Task<IActionResult> OnPostOpenBookCourtModal(int courtID, DateTime date, int timeSlot, bool isBooked)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                return RedirectToPage(@"/User/Login");
            }

            try
            {
                CurrentCourt = await _courtDB.GetCourtByIDAsync(courtID);
                CurrentDate = date;
                CurrentTimeSlot = timeSlot;
                IsBooked = isBooked;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            await OnPageReload();
            return Page();
        }

        public async Task<IActionResult> OnPostBookCourt(int courtID, DateTime date, int timeSlot)
        {
            if (int.TryParse(HttpContext.Session.GetString("User")?.Split('|')[0], out int memberID))
            {
                try
                {
                    // Make sure the member has'nt already booked up to their max bookings
                    if (MaxBookings > await GetMyCurrentBookingsCount(memberID))
                    {
                        ICourtBooking newCourtBooking = new CourtBooking(0, courtID, DateOnly.FromDateTime(date), timeSlot, null, memberID, null);
                        await CourtBookingDB.CreateCourtBookingAsync(newCourtBooking);
                        
                        MessageSuccess = $"Du har nu booket bane {courtID} kl. {timeSlot + HourToBeginFrom}:00 d. {date.ToShortDateString()}";
                    }
                    else MessageDanger = $"Du har ikke flere tilgængelige bookinger tilbage";
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }

            await OnPageReload();
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveCourtBooking(int courtID, DateTime date, int timeSlot)
        {
            if (int.TryParse(HttpContext.Session.GetString("User")?.Split('|')[0], out int memberID))
            {
                try
                {
                    await GenerateCurrentBookingsDict(); // Make sure the bookings are actually loaded before trying to remove the booking.

                    var lookupKey = (courtID, DateOnly.FromDateTime(date), timeSlot);
                    if (CurrentBookingsDict.TryGetValue(lookupKey, out ICourtBooking? courtBooking))
                    {
                        await CourtBookingDB.DeleteCourtBookingAsync(courtBooking.ID);

                        MessageDanger = $"Du har nu slettet din booking: bane {courtID} kl. {timeSlot + HourToBeginFrom}:00 d. {date.ToShortDateString()}";
                    }

                    CurrentBookingsDict.Clear(); // Clear the dictionary before we continue, to make sure the PageReload reloads the dictionary from scatch.
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }

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
