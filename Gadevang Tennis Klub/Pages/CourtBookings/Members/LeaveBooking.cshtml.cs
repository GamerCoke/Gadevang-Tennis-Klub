using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings.Members
{
    public class LeaveBookingModel : PageModel
    {
        public int BookingId { get; set; }
        public ICourtBookingDB CourtBookingDB { get; set; }
        public LeaveBookingModel(ICourtBookingDB courtBookingDB)
        {
            CourtBookingDB = courtBookingDB;
        }
        public async Task<IActionResult> OnGet(int bookingId)
        {
            string? user = HttpContext.Session.GetString("User");
            BookingId = bookingId;
            int bookerId = (int)(await CourtBookingDB.GetCourtBookingByIDAsync(bookingId)).Member_ID;

            int userId = int.Parse(HttpContext.Session.GetString("User").Split('|')[0]);
            if (user == null || bookerId == userId)
                return RedirectToPage(@"/User/Login");

            if (!(await CourtBookingDB.GetCourtBookingByIDAsync(bookingId)).Participants
                .ToList()
                .ConvertAll(m=>m.Id)
                .Contains(userId))
                return RedirectToPage(@"/User/MyPage");

            return Page();
        }
        public async Task<IActionResult> OnPost(int bookingId)
        {
            await CourtBookingDB.RemovePartisipantAsync(bookingId, int.Parse(HttpContext.Session.GetString("User").Split('|')[0]));
            return RedirectToPage(@"/User/MyPage");
        }
    }
}
