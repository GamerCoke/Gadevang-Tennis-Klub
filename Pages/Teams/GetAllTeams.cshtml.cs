using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gadevang_Tennis_Klub.Pages.Teams
{
    public class GetAllTeamsModel : PageModel
    {
        private readonly ITeamDB _teamDB;
        private readonly ITrainerDB _trainerDB;
        private readonly ITeamBookingDB _teamBookingDB;
        private readonly IMemberDB _memberDB;

        public GetAllTeamsModel(ITeamDB teamDB, ITrainerDB trainerDB, ITeamBookingDB teamBookingDB, IMemberDB memberDB)
        {
            _teamDB = teamDB;
            _trainerDB = trainerDB;
            _teamBookingDB = teamBookingDB;
            _memberDB = memberDB;
        }

        public List<ITeam> Teams { get; set; } = new();
        public List<ITeamBooking> TeamBookings { get; set; } = new();
        public Dictionary<int, string> TrainerNames { get; set; } = new();
        public Dictionary<int, int> TeamMemberCounts { get; set; } = new();
        public Dictionary<int, List<IMember>> TeamMembers { get; set; } = new();
        public bool IsAdmin { get; set; }
        public int CurrentUserId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userSession = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(userSession))
            {
                var parts = userSession.Split('|');
                if (parts.Length > 1 && bool.TryParse(parts[1], out bool isAdmin))
                {
                    IsAdmin = isAdmin;
                }
            }

            int? memberId = ExtractMemberIDFromSession();
            if (memberId == null)
            {
                // This redirect now works correctly — no exception thrown before it.
                return RedirectToPage("/User/Login");
            }

            CurrentUserId = memberId.Value;

            Teams = await _teamDB.GetAllTeamsAsync();
            TeamBookings = await _teamBookingDB.GetAllTeamBookingAsync();

            var trainerIds = Teams.Select(t => t.TrainerId).Distinct().ToList();
            foreach (var id in trainerIds)
            {
                var trainer = await _trainerDB.GetTrainerByIDAsync(id);
                TrainerNames[id] = trainer?.Name ?? "Unknown Trainer";
            }

            TeamMemberCounts = TeamBookings
                .GroupBy(b => b.Team_ID)
                .ToDictionary(g => g.Key, g => g.Count());

            if (IsAdmin)
            {
                foreach (var team in Teams)
                {
                    if (team.ID != null)
                    {
                        TeamMembers[team.ID.Value] = await _teamBookingDB.GetMembersByTeamAsync(team.ID.Value, _memberDB);
                    }
                }
            }

            Teams = (await _teamDB.GetAllTeamsAsync())
    .OrderBy(t => DayOfWeekValue(t.ActiveDay))
    .ThenBy(t => t.Name) // optional secondary sort
    .ToList();


            return Page();
        }


        public async Task<IActionResult> OnPostDeleteAsync(int teamID)
        {
            if (teamID <= 0)
            {
                TempData["JoinError"] = "Team is full or unavailable.";
                return RedirectToPage(); // This avoids rehydration issues
            }

            bool success = await _teamDB.DeleteTeamAsync(teamID);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, $"Failed to delete team with ID {teamID}.");
            }

            return RedirectToPage(); // Refresh this page
        }

        public int GetMemberCountForTeam(int teamId)
        {
            return TeamMemberCounts.TryGetValue(teamId, out int count) ? count : 0;
        }

        public async Task<IActionResult> OnPostJoinTeamAsync(int teamID)
        {
            try
            {
                string? currentUser = HttpContext.Session.GetString("User");
                if (string.IsNullOrEmpty(currentUser))
                    return RedirectToPage("/User/Login");

                int? memberID = ExtractMemberIDFromSession();
                if (memberID == null)
                    return RedirectToPage("/User/Login");

                var team = await _teamDB.GetTeamByIDAsync(teamID);
                if (team == null)
                {
                    ModelState.AddModelError(string.Empty, "Team not found.");
                    await OnGetAsync();
                    return Page();
                }

                var bookings = await _teamBookingDB.GetMembersByTeamAsync(teamID, _memberDB);
                if (bookings.Count >= team.Capacity)
                {
                    ModelState.AddModelError(string.Empty, "Team is full, cannot join.");
                    await OnGetAsync();
                    return Page();
                }

                if (bookings.Any(b => b.Id == memberID.Value))
                {
                    ModelState.AddModelError(string.Empty, "You are already a member of this team.");
                    await OnGetAsync();
                    return Page();
                }

                ITeamBooking newBooking = new TeamBooking
                {
                    Member_ID = memberID.Value,
                    Team_ID = teamID
                };

                await _teamBookingDB.CreateTeamBookingAsync(newBooking);

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error joining team: {ex.Message}");
                await OnGetAsync();
                return RedirectToPage("GetAllTeams");
            }
        }


        public async Task<IActionResult> OnPostLeaveTeamAsync(int teamID)
        {
            string? currentUser = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(currentUser))
            {
                return RedirectToPage("/User/Login");
            }

            int? memberID = ExtractMemberIDFromSession();
            if (memberID == null)
            {
                return RedirectToPage("/User/Login");
            }

            int? bookingID = await _teamBookingDB.GetTeamBookingIDAsync(teamID, memberID.Value);
            if (bookingID.HasValue)
            {
                await _teamBookingDB.DeleteTeamBookingAsync(bookingID.Value);
            }

            return RedirectToPage();
        }

        public bool UserIsInTeam(int teamId, int? userId)
        {
            if (userId == null) return false;
            return TeamBookings.Any(tb => tb.Team_ID == teamId && tb.Member_ID == userId.Value);
        }

        public int? ExtractMemberIDFromSession()
        {
            string? userSession = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userSession)) return null;

            var parts = userSession.Split('|');
            if (int.TryParse(parts[0], out int id))
            {
                return id;
            }

            return null;
        }
        private int DayOfWeekValue(string day)
        {
            return day?.ToLowerInvariant() switch
            {
                "mandag" => 1,
                "tirsdag" => 2,
                "onsdag" => 3,
                "torsdag" => 4,
                "fredag" => 5,
                "lørdag" => 6,
                "søndag" => 7,
                _ => 8
            };
        }
    }
}
