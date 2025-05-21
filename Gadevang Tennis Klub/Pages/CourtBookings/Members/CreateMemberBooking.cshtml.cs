using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings.Members
{
    public class CreateMemberBookingModel : PageModel
    {
        public ICourtBookingDB CourtBookingDB { get; set; }
        public ICourtDB CourtDB { get; set; }
        public int Id { get; set; }
        public string? Message { get; set; }
        #region Booking Values
        [BindProperty]
        public DateOnly Date { get; set; }
        [BindProperty]
        public int CourtId { get; set; }
        #endregion
        public int Timeslot { get; set; }

        public CreateMemberBookingModel(ICourtBookingDB courtBookingDB, ICourtDB courtDB)
        {
            CourtBookingDB = courtBookingDB;
            CourtDB = courtDB;
            CourtId = -1;
            Date = DateOnly.FromDateTime(DateTime.Now);
        }


        public IActionResult OnGetFromCourt(int courtId)
        {
            CourtId = courtId;
            return CheckLogin();
        }

        public IActionResult OnGet()
        {
            return CheckLogin();
        }
        private IActionResult CheckLogin()
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else
                return Page();
        }
        public async Task<IActionResult> OnPost(int courtId, int timeslot)
        {
            Id = int.Parse(HttpContext.Session.GetString("User").Split('|')[0]);
            CourtId = courtId;
            Timeslot = timeslot;
            ICourtBooking Booking = new CourtBooking(0, courtId, Date, timeslot, null, Id, null);

            if (Booking.Timeslot < 0 || Booking.Timeslot > 17)
            {
                Message = "Tidsslotet skal v�re mellem 0 og 17";
                return Page();
            }

            try
            {
                if (!ModelState.IsValid || !await CourtBookingDB.CreateCourtBookingAsync(Booking))
                    return Page();
            }
            catch (Exception ex)
            {
                Message = "Denne bane er booket p� dette tidspunkt.";
                return Page();
            }

            return RedirectToPage("/User/MyPage");
        }
    }
}
