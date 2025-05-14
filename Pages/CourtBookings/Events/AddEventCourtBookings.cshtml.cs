using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings.Events
{
    public class AddEventCourtBookingsModel : PageModel
    {
        public ICourtBookingDB CourtBookingDB { get; set; }
        public ICourtDB CourtDB { get; set; }
        public IEventDB EventDB { get; set; }
        public string? Message { get; set; }
        public int CourtId { get; set; }
        public int EventId { get; set; }
        public int Timeslot { get; set; }
        public AddEventCourtBookingsModel(ICourtBookingDB courtBookingDB, ICourtDB courtDB, IEventDB eventDB)
        {
            CourtBookingDB = courtBookingDB;
            CourtDB = courtDB;
            EventDB = eventDB;
            CourtId = -1;
            EventId = -1;
        }

        public IActionResult OnGet()
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (!bool.Parse(user.Split('|')[1]))
                return RedirectToPage(@"/User/MyPage");

            return Page();
        }

        public async Task<IActionResult> OnPost(int eventId, int courtId, int timeslot)
        {
            CourtId = courtId;
            EventId = eventId;
            Timeslot = timeslot;
            ICourtBooking booking = new CourtBooking(0, courtId, DateOnly.FromDateTime((await EventDB.GetEventByIDAsync(eventId)).Start), timeslot, null, null, eventId);

            try
            {
                if (!ModelState.IsValid || !await CourtBookingDB.CreateCourtBookingAsync(booking))
                    return Page();
            }
            catch (Exception ex)
            {
                Message = $"Denne bane er booket på dette tidspunkt.";
                return Page();
            }

            return RedirectToPage(@"/CourtBookings/Events/ViewEventCourtBookings");
        }
    }
}

