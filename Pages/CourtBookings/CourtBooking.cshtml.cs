using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings
{
    public class CourtBookingModel : PageModel
    {
        ICourtDB _courtDB;

        public ICourtBookingDB CourtBookingDB { get; private set; }
        public ITeamDB TeamDB { get; private set; }
        public IEventDB EventDB { get; private set; }
        public IMemberDB MemberDB { get; private set; }


        // For the message popup when successfully booking or removing bookings
        public string MessageSuccess { get; set; }
        public string MessageDanger { get; set; }
        public string MessageModalDanger { get; set; }


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
        public List<int>? ParticipantIDs { get; set; }


        [BindProperty] public bool UseBallMachine { get; set; }



        public CourtBookingModel(ICourtDB courtDB, ICourtBookingDB courtBookingDB, ITeamDB teamDB, IEventDB eventDB, IMemberDB memberDB)
        {
            _courtDB = courtDB;
            CourtBookingDB = courtBookingDB;
            TeamDB = teamDB;
            EventDB = eventDB;
            MemberDB = memberDB;

            Areas = new List<string>();
            CourtsDict = new Dictionary<string, List<ICourt>>();
            CurrentBookingsDict = new Dictionary<(int CourtID, DateOnly Date, int TimeSlot), ICourtBooking>();
        }

        public async Task OnGetAsync()
        {
            if (SelectedDate == default)
                SelectedDate = DateTime.Today;

            await OnPageReloadAsync();
        }

        private async Task OnPageReloadAsync()
        {
            await LinkCourtsToAreasAsync();
            await GenerateCurrentBookingsDictAsync();

            // Create dropdown list for areas.
            CreateAreaSelectList();
            CurrentArea = Areas[SelectedArea];

            await GetCurrentWeekDaysAsync(SelectedDate);
        }

        private async Task LinkCourtsToAreasAsync()
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
        private async Task GenerateCurrentBookingsDictAsync()
        {
            List<ICourtBooking> currentCourtBookings = await CourtBookingDB.GetAllCourtBookingsAsync();
            foreach (ICourtBooking courtBooking in currentCourtBookings)
            {
                CurrentBookingsDict.Add((courtBooking.Court_ID, courtBooking.Date, courtBooking.Timeslot), courtBooking);
            }
        }


        private async Task GetCurrentWeekDaysAsync(DateTime day)
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

        public async Task<int> GetMyCurrentBookingsCountAsync(int memberID)
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

            if (CurrentBookingsDict.TryGetValue(lookupKey, out ICourtBooking? courtBooking) && courtBooking != null)
            {
                return courtBooking.Member_ID == memberID
                    || courtBooking.Participants?.Any(p => p.Id == memberID) == true;
            }

            return false;
        }

        public async Task<IMember> GetOrganizer(ICourt court, DateTime date, int timeSlot)
        {
            if (!int.TryParse(HttpContext.Session.GetString("User")?.Split('|')[0], out int memberID)) return null; // return early if a user could not be found.

            var lookupKey = (court.ID, DateOnly.FromDateTime(date), timeSlot);
            if (!CurrentBookingsDict.TryGetValue(lookupKey, out ICourtBooking? courtBooking)) return null;

            if (courtBooking?.Member_ID is int organizerID)
            {
                return await MemberDB.GetMemberByIDAsync(organizerID);
            }

            return null;
        }

        public async Task<List<IMember>> GetAllAvailableMembers()
        {
            if (!int.TryParse(HttpContext.Session.GetString("User")?.Split('|')[0], out int memberID)) return new List<IMember>();

            List<IMember> allMembers = await MemberDB.GetAllMembersAsync();
            List<IMember> availableMembers = new List<IMember>();

            foreach (IMember member in allMembers)
            {
                if (member.Id != memberID && ParticipantIDs != null && !ParticipantIDs.Contains(member.Id))
                    availableMembers.Add(member);
            }

            return availableMembers;
        }

        public async Task OnPostAddMemberToBooking(int courtID, DateTime date, int timeSlot, bool isBooked, int memberId, List<int> participantIDs)
        {
            if (memberId == 0) // If no member was chosen.
            {
                await OnPostOpenBookCourtModalAsync(courtID, date, timeSlot, isBooked, participantIDs);
                return;
            }

            try
            {
                participantIDs.Add(memberId);

                if (isBooked)
                {
                    await GenerateCurrentBookingsDictAsync(); // Make sure the current bookings are loaded before trying to retrieve the booking data.

                    var lookupKey = (courtID, DateOnly.FromDateTime(date), timeSlot);
                    if (CurrentBookingsDict.TryGetValue(lookupKey, out ICourtBooking? courtBooking))
                    {
                        await CourtBookingDB.AddPartisipantAsync(courtBooking.ID, memberId);                      
                    }

                    CurrentBookingsDict.Clear(); // Clear the dictionary before we continue, to make sure the PageReload reloads the dictionary from scatch.
                }

                ModelState.Remove(nameof(UseBallMachine)); // Make sure the ball machine isn't checked when adding participants to the booking.
                UseBallMachine = false;

                await OnPostOpenBookCourtModalAsync(courtID, date, timeSlot, isBooked, participantIDs);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
        public async Task OnPostRemoveMemberFromBooking(int courtID, DateTime date, int timeSlot, bool isBooked, int memberId, List<int> participantIDs)
        {
            try
            {
                participantIDs.Remove(memberId);

                if (isBooked)
                {
                    await GenerateCurrentBookingsDictAsync(); // Make sure the current bookings are loaded before trying to retrieve the booking data.

                    var lookupKey = (courtID, DateOnly.FromDateTime(date), timeSlot);
                    if (CurrentBookingsDict.TryGetValue(lookupKey, out ICourtBooking? courtBooking))
                    {
                        await CourtBookingDB.RemovePartisipantAsync(courtBooking.ID, memberId);
                    }

                    CurrentBookingsDict.Clear(); // Clear the dictionary before we continue, to make sure the PageReload reloads the dictionary from scatch.
                }

                await OnPostOpenBookCourtModalAsync(courtID, date, timeSlot, isBooked, participantIDs);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }

        public async Task OnPostNewAreaChosenAsync()
        {
            await OnPageReloadAsync();
        }

        public async Task OnPostNewDateChosenAsync()
        {
            await OnPageReloadAsync();
        }

        public async Task<IActionResult> OnPostOpenBookCourtModalAsync(int courtID, DateTime date, int timeSlot, bool isBooked, List<int>? participantIDs = null)
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

                if (IsBooked)
                {
                    await GenerateCurrentBookingsDictAsync(); // Make sure the current bookings are loaded before trying to retrieve the participant data.

                    var lookupKey = (courtID, DateOnly.FromDateTime(date), timeSlot);
                    CurrentBookingsDict.TryGetValue(lookupKey, out ICourtBooking? courtBooking);
                    
                    ParticipantIDs = courtBooking?.Participants?.Select(m => m.Id).ToList() ?? new List<int>();

                    ModelState.Remove(nameof(UseBallMachine)); // Reset the UseBallMachine checkbox, so we can set our own value to it.
                    UseBallMachine = ParticipantIDs.Count == 0;

                    CurrentBookingsDict.Clear(); // Clear the dictionary before we continue, to make sure the PageReload reloads the dictionary from scatch.
                }
                else ParticipantIDs = participantIDs ?? new List<int>();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            await OnPageReloadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostBookCourt(int courtID, DateTime date, int timeSlot, List<int>? participantIDs = null)
        {
            ParticipantIDs = participantIDs;
            bool hasParticipants = participantIDs?.Count > 0;

            try
            {
                if (int.TryParse(HttpContext.Session.GetString("User")?.Split('|')[0], out int memberID))
                {
                    // Make sure the member has'nt already booked up to their max bookings
                    if (await GetMyCurrentBookingsCountAsync(memberID) >= MaxBookings)
                    {
                        MessageDanger = "Du har ikke flere tilgængelige bookinger tilbage";
                        await OnPageReloadAsync();
                        return Page();
                    }

                    if (!hasParticipants && !UseBallMachine)
                    {
                        MessageModalDanger = $"Tilføj venligst min. 1 deltager til din booking, eller vælg at du ønsker at booke med boldmaskinen, for at kunne booke en bane";
                        await OnPostOpenBookCourtModalAsync(courtID, date, timeSlot, false, participantIDs);
                        return Page();
                    }

                    await GenerateCurrentBookingsDictAsync(); // Make sure the current bookings are loaded before trying to book the court.

                    // Create and add the neew booking to the DB.
                    ICourtBooking newCourtBooking = new CourtBooking(0, courtID, DateOnly.FromDateTime(date), timeSlot, null, memberID, null);
                    await CourtBookingDB.CreateCourtBookingAsync(newCourtBooking);

                    // Attempt to retrieve the ID of the new cout booking.
                    int newBookingID = await GetNewCourtBookingIDAsync();

                    // Add participants if there is any...
                    if (hasParticipants)
                    {
                        foreach (var participantID in ParticipantIDs)
                        {
                            await CourtBookingDB.AddPartisipantAsync(newBookingID, participantID);
                        }
                    }
                        
                    MessageSuccess = $"Du har nu booket bane {courtID} kl. {timeSlot + HourToBeginFrom}:00 d. {date.ToShortDateString()}";

                    CurrentBookingsDict.Clear(); // Clear the dictionary before we continue, to make sure the PageReload reloads the dictionary from scatch.
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            await OnPageReloadAsync();
            return Page();
        }

        private async Task<int> GetNewCourtBookingIDAsync()
        {
            // Retrieve all the court bookings currently in the database.
            List<ICourtBooking> allBookings = await CourtBookingDB.GetAllCourtBookingsAsync();

            // Then match this list with the dict with currentBookings to find the newly created one.
            var newBookings = allBookings
                .Where(ab => !CurrentBookingsDict.Values.Any(cb =>
                    cb.Court_ID == ab.Court_ID &&
                    cb.Date == ab.Date &&
                    cb.Timeslot == ab.Timeslot &&
                    cb.Member_ID == ab.Member_ID))
                .ToList();

            return newBookings.FirstOrDefault().ID;
        }

        public async Task<IActionResult> OnPostRemoveCourtBooking(int courtID, DateTime date, int timeSlot, List<int>? participantIDs = null)
        {
            // Make sure there isn't any participants when trying to remove the booking.
            if (participantIDs != null && participantIDs.Count > 0)
            {
                MessageModalDanger = "Fjern venligst alle deltagere før du sletter bookingen.";
                await OnPostOpenBookCourtModalAsync(courtID, date, timeSlot, true, participantIDs);
                return Page();
            }

            if (int.TryParse(HttpContext.Session.GetString("User")?.Split('|')[0], out int memberID))
            {
                try
                {
                    await GenerateCurrentBookingsDictAsync(); // Make sure the bookings are actually loaded before trying to remove the booking.

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

            await OnPageReloadAsync();
            return Page();
        }

        public async Task OnPostPreviousAsync()
        {
            SelectedDate = SelectedDate.AddDays(-7);
            await OnPageReloadAsync();
        }

        public async Task OnPostNextAsync()
        {
            SelectedDate = SelectedDate.AddDays(7);
            await OnPageReloadAsync();
        }

        public async Task OnPostTodayAsync()
        {
            SelectedDate = DateTime.Today;
            await OnPageReloadAsync();
        }
    }
}
