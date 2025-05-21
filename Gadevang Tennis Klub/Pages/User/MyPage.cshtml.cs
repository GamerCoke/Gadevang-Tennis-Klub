using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.User
{
    public class MyPageModel : PageModel
    {
        public IMemberDB MemberDB { get; set; }
        public ICourtBookingDB CourtBookingDB { get; set; }
        public ICourtDB CourtDB { get; set; }
        public IEventBookingDB EventBookingDB { get; set; }
        public IEventDB EventDB { get; set; }
        public ITeamDB TeamDB { get; set; }
        public ITrainerDB TrainerDB { get; set; }
        public IMember Member { get; set; }
        public MyPageModel(IMemberDB memberDB, ICourtBookingDB courtBookingDB, IEventBookingDB eventBookingDB, IEventDB eventDB, ITeamDB teamDB, ICourtDB courtDB, ITrainerDB trainerDB)
        {
            MemberDB = memberDB;
            CourtBookingDB = courtBookingDB;
            EventBookingDB = eventBookingDB;
            EventDB = eventDB;
            TeamDB = teamDB;
            CourtDB = courtDB;
            TrainerDB = trainerDB;
        }

        public async Task<IActionResult> OnGet()
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"Login");

            Member = await MemberDB.GetMemberByIDAsync(int.Parse(user.Split('|')[0]));

            return Page();
        }
    }
}
