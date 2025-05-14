using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings.Teams
{
    public class ViewTeamCourtBookingsModel : PageModel
    {
        private ICourtBookingDB _courtBookingDB { get; set; }
        private ITeamDB _teamDB { get; set; }
        public ViewTeamCourtBookingsModel(ICourtBookingDB courtBookingDB, ITeamDB teamDB)
        {
            _courtBookingDB = courtBookingDB;
            _teamDB = teamDB;
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
        public async Task<IEnumerable<ICourtBooking>> GetAllTeamCourtBookingsAsync()
        {
            IEnumerable<ICourtBooking> bookings = (await _courtBookingDB.GetAllCourtBookingsAsync()).Where(c => c.Team_ID != null);
            
            return bookings;
        }
        public async Task<ITeam> GetTeamAsync(int id)
        {
            return await _teamDB.GetTeamByIDAsync(id);
        }
    }
}
