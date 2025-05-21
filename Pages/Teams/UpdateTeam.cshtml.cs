using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gadevang_Tennis_Klub.Pages.Teams
{
    public class UpdateTeamModel : PageModel
    {
        private readonly ITeamDB _teamDB;
        private readonly ITrainerDB _trainerDB;

        public UpdateTeamModel(ITeamDB teamDB, ITrainerDB trainerDB)
        {
            _teamDB = teamDB;
            _trainerDB = trainerDB;
        }

        [BindProperty]
        public Team Team { get; set; }

        public SelectList TrainerOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var team = await _teamDB.GetTeamByIDAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            Team = new Team
            {
                ID = team.ID,
                Name = team.Name,
                Description = team.Description,
                Capacity = team.Capacity,
                Price = team.Price,
                ActiveDay = team.ActiveDay,
                TrainerId = team.TrainerId
            };

            var trainers = await _trainerDB.GetAllTrainersAsync();
            TrainerOptions = new SelectList(trainers, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var trainers = await _trainerDB.GetAllTrainersAsync();
            TrainerOptions = new SelectList(trainers, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool updated = await _teamDB.UpdateTeamAsync(Team);
            if (!updated)
            {
                ModelState.AddModelError("", "Could not update the team.");
                return Page();
            }

            return RedirectToPage("GetAllTeams");
        }
    }
}
