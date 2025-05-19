using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Trainers
{
    public class PublicTrainerOverviewModel : PageModel
    {
        public ITeamDB TeamDB { get; private set; }
        public ITrainerDB TrainerDB { get; private set; }

        public PublicTrainerOverviewModel(ITrainerDB trainerDB, ITeamDB teamDB)
        {
            TrainerDB = trainerDB;
            TeamDB = teamDB;
        }

        public void OnGet()
        {
        }
    }
}
