using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings.Events
{
    public class RemoveEventCourtBookingsModel : PageModel
    {
        public int BookingId { get; set; }
        private ICourtBookingDB _courtBookingDB;
        private IEventDB _eventDB;
        public RemoveEventCourtBookingsModel(ICourtBookingDB courtBookingDB, IEventDB eventDB)
        {
            _courtBookingDB = courtBookingDB;
            _eventDB = eventDB;
        }
        public IActionResult OnGet(int bookingId)
        {
            BookingId = bookingId;
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (!bool.Parse(user.Split('|')[1]))
                return RedirectToPage(@"/User/MyPage");

            return Page();
        }
        public async Task<IActionResult> OnPost(int bookingId)
        {
            BookingId = bookingId;
            await _courtBookingDB.DeleteCourtBookingAsync(BookingId);

            return RedirectToPage(@"/CourtBookings/Events/ViewEventCourtBookings");
        }
        public async Task<ICourtBooking> GetBookingAsync()
        {
            return await _courtBookingDB.GetCourtBookingByIDAsync(BookingId);
        }
        public async Task<IEvent> GetEventAsync(int eventId)
        {
            return await _eventDB.GetEventByIDAsync(eventId);
        }
    }
}
