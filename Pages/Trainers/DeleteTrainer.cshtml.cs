using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Gadevang_Tennis_Klub.Pages.Trainers
{
    public class DeleteTrainerModel : PageModel
    {
        public string? Message;
        public ITrainer Trainer { get; set; }
        private ITrainerDB _db;
        public DeleteTrainerModel(ITrainerDB db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGet(int trainerID)
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (!bool.Parse(user.Split('|')[1]))
                return RedirectToPage(@"/Trainers/ReadAllTrainers");
            Trainer = await _db.GetTrainerByIDAsync(trainerID);
            return Page();
        }
        public async Task<IActionResult> OnPost(int trainerID)
        {
            try
            {
                await _db.DeleteTrainerAsync(trainerID);
                return RedirectToPage(@"/Trainers/ReadAllTrainers");
            }
            catch (SqlException)
            {
                Trainer = await _db.GetTrainerByIDAsync(trainerID);
                Message = "Træneren er påsat et hold og kan derfor ikke slettes";
                return Page();
            }
        }
    }
}
