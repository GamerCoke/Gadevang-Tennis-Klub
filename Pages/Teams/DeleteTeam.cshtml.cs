using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Teams
{
    public class DeleteTeamModel : PageModel
    {
        private readonly ITeamDB _teamDB;

        public DeleteTeamModel(ITeamDB teamDB)
        {
            _teamDB = teamDB;
        }

        public List<ITeam> Teams { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? TeamID { get; set; }

        public async Task OnGetAsync()
        {
            Teams = await _teamDB.GetAllTeamsAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int teamID)
        {
            Console.WriteLine($">>> Received teamID: {teamID}");

            if (teamID <= 0)
            {
                return BadRequest();
            }

            bool success = await _teamDB.DeleteTeamAsync(teamID);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete the team.");
                await OnGetAsync();
                return Page();
            }

            return RedirectToPage(); // Reloads the same page to reflect deletion
        }
    }
}
