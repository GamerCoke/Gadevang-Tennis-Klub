using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Pages.Trainers
{
    public class UpdateTrainerModel : PageModel
    {

        [BindProperty]
        public ITrainer Trainer { get; set; }

        public string? Message;
        private ITrainerDB _db;
        public UpdateTrainerModel(ITrainerDB db)
        {
            Message = null;
            _db = db;
        }
        public async Task<IActionResult> OnGet(int trainerId)
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (!bool.Parse(user.Split('|')[1]))
                return RedirectToPage(@"/Trainers/ReadAllTrainers");
            Trainer = await _db.GetTrainerByIDAsync(trainerId);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                await _db.UpdateTrainerAsync(Trainer);
                return RedirectToPage(@"/Trainers/ReadAllTrainers");
            }
            else
            {
                Message = "Træner blev ikke opdateret";
                return Page();
            }
        }
    }
}
