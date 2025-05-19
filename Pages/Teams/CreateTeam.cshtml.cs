using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Gadevang_Tennis_Klub.Pages.Teams
{
    public class CreateTeamModel : PageModel
    {
        private readonly ITeamDB _teamDB;
        private readonly ITrainerDB _trainerDB;

        public string? CurrentUser { get; private set; }
        public bool IsAdmin { get; private set; }

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
            CurrentUser = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(CurrentUser))
            {
                return RedirectToPage(@"/User/Login");
            }

            IsAdmin = bool.Parse(CurrentUser.Split('|')[1]);
            if (!IsAdmin)
            {
                return RedirectToPage(@"/Index");
            }

            Team = new Team();

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

            ITeam team = Team;

            try
            {
                bool created = await _teamDB.CreateTeamAsync(Team);
                if (!created)
                {
                    ModelState.AddModelError(string.Empty, "Could not create the team.");
                    return Page();
                }

                // Redirect to the team listing page after successful creation
                return RedirectToPage(@"GetAllTeams");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return Page();
            }
        }
    }
}