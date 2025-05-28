using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Gadevang_Tennis_Klub.Pages.Teams
{
    public class CreateTeamModel : PageModel
    {
        private readonly ITeamDB _teamDB;
        private readonly ITrainerDB _trainerDB;

        [BindProperty]
        public Team Team { get; set; }

        public SelectList TrainerOptions { get; set; }

        public CreateTeamModel(ITeamDB teamDB, ITrainerDB trainerDB)
        {
            _teamDB = teamDB;
            _trainerDB = trainerDB;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            TrainerOptions = new SelectList(await _trainerDB.GetAllTrainersAsync(), "Id", "Name");
            Team = new Team();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            TrainerOptions = new SelectList(await _trainerDB.GetAllTrainersAsync(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool created = await _teamDB.CreateTeamAsync(Team);
            if (!created)
            {
                ModelState.AddModelError(string.Empty, "Could not create the team.");
                return Page();
            }

            return RedirectToPage("GetAllTeams");
        }
    }
}
