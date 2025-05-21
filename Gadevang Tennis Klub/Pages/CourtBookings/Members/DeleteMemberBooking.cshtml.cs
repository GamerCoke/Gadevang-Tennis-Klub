using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings
{
    public class DeleteMemberBookingModel : PageModel
    {
        public int BookingId { get; set; }
        public ICourtBookingDB CourtBookingDB { get; set; }
        public DeleteMemberBookingModel(ICourtBookingDB courtBookingDB)
        {
            CourtBookingDB = courtBookingDB;
        }
        public async Task<IActionResult> OnGet(int bookingId)
        {
            string? user = HttpContext.Session.GetString("User");
            BookingId = bookingId;
            int bookerId = (int)(await CourtBookingDB.GetCourtBookingByIDAsync(bookingId)).Member_ID;
            if (user == null || bookerId != int.Parse(HttpContext.Session.GetString("User").Split('|')[0]))
                return RedirectToPage(@"/User/Login");
            else
                return Page();
        }
        public async Task<IActionResult> OnPost(int bookingId)
        {
            await CourtBookingDB.DeleteCourtBookingAsync(bookingId);
            return RedirectToPage(@"/User/MyPage");
        }
    }
}
