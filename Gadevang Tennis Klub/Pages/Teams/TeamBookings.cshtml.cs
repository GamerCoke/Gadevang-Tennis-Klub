using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gadevang_Tennis_Klub.Pages.Teams
{
    public class TeamBookingsModel : PageModel
    {
        private readonly ITeamBookingDB _teamBookingDB;
        private readonly IMemberDB _memberDB;

        public TeamBookingsModel(ITeamBookingDB teamBookingDB, IMemberDB memberDB)
        {
            _teamBookingDB = teamBookingDB;
            _memberDB = memberDB;
        }

        public List<IMember> Members { get; set; } = new();
        public int TeamId { get; set; }
        public bool IsAdmin { get; set; }

        public async Task<IActionResult> OnGetAsync(int teamID)
        {
            TeamId = teamID;

            var session = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(session))
            {
                var parts = session.Split('|');
                if (parts.Length > 1 && bool.TryParse(parts[1], out bool isAdmin))
                {
                    IsAdmin = isAdmin;
                }
            }

            Members = await _teamBookingDB.GetMembersByTeamAsync(teamID, _memberDB);
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveMemberAsync(int memberId, int teamId)
        {
            var session = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(session) || !session.Contains("True")) // crude IsAdmin check
            {
                return Forbid();
            }

            int? bookingId = await _teamBookingDB.GetTeamBookingIDAsync(teamId, memberId);
            if (bookingId.HasValue)
            {
                await _teamBookingDB.DeleteTeamBookingAsync(bookingId.Value);
            }

            return RedirectToPage(new { teamID = teamId });
        }
    }
}
