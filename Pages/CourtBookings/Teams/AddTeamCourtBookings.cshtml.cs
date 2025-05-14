using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings.Teams
{
    public class AddTeamCourtBookingsModel : PageModel
    {
        public ICourtBookingDB CourtBookingDB { get; set; }
        public ICourtDB CourtDB { get; set; }
        public ITeamDB TeamDB { get; set; }
        public string? Message { get; set; }
        #region Booking Values
        [BindProperty]
        public DateOnly Date { get; set; }
        #endregion
        public int CourtId { get; set; }
        public int TeamId { get; set; }
        public int Timeslot { get; set; }
        public AddTeamCourtBookingsModel(ICourtBookingDB courtBookingDB, ICourtDB courtDB, ITeamDB teamDB)
        {
            CourtBookingDB = courtBookingDB;
            CourtDB = courtDB;
            TeamDB = teamDB;
            CourtId = -1;
            TeamId = -1;
            Date = DateOnly.FromDateTime(DateTime.Now);
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

        public async Task<IActionResult> OnPost(int teamId, int courtId, int timeslot)
        {
            CourtId = courtId;
            TeamId = teamId;
            Timeslot = timeslot;
            ICourtBooking booking = new CourtBooking(0, courtId, Date, timeslot, teamId, null, null);

            try
            {
                if (!ModelState.IsValid || !await CourtBookingDB.CreateCourtBookingAsync(booking))
                    return Page();
            }
            catch (Exception ex)
            {
                Message = $"Denne bane er booket på dette tidspunkt. {timeslot} {Date} {ex}";
                return Page();
            }

            return RedirectToPage(@"/CourtBookings/Teams/ViewTeamCourtBookings");
        }
    }
}
