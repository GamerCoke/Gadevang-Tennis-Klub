using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings.Events
{
    public class ViewEventCourtBookingsModel : PageModel
    {
        private ICourtBookingDB _courtBookingDB { get; set; }
        private IEventDB _eventDB { get; set; }
        private ICourtDB _cbd { get; set; }
        public ViewEventCourtBookingsModel(ICourtBookingDB courtBookingDB, IEventDB eventDB, ICourtDB cbd)
        {
            _courtBookingDB = courtBookingDB;
            _eventDB = eventDB;
            _cbd = cbd;
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
        public async Task<IEnumerable<ICourtBooking>> GetAllEventCourtBookingsAsync()
        {
            IEnumerable<ICourtBooking> bookings = (await _courtBookingDB.GetAllCourtBookingsAsync()).Where(c => c.Event_ID != null);

            return bookings;
        }
        public async Task<ICourt> GetCourtAsync(int id)
        {
            return await _cbd.GetCourtByIDAsync(id);
        }
        public async Task<IEvent> GetEventAsync(int id)
        {
            return await _eventDB.GetEventByIDAsync(id);
        }
    }
}
