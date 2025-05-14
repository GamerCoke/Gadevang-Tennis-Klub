using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Trainers
{
    public class ReadAllTrainersModel : PageModel
    {
        public ITeamDB TeamDB;
        public ITrainerDB TrainerDB;
        public bool IsAdmin;
        public ReadAllTrainersModel(ITrainerDB trainerDB, ITeamDB teamDB)
        {
            TrainerDB = trainerDB;
            TeamDB = teamDB;
        }

        public void OnGet()
        {
            string? user = HttpContext.Session.GetString("User");
            if (user != null)
                IsAdmin = bool.Parse(user.Split('|')[1]);
            else
                IsAdmin = false;
        }
    }
}
