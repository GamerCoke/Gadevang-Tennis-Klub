using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Pages.Trainers
{
    public class CreateTrainerModel : PageModel
    {
        [BindProperty]
        public Trainer Trainer { get; set; }

        public string? Message;
        private ITrainerDB _db;
        public CreateTrainerModel(ITrainerDB db)
        {
            Message = null;
            _db = db;
        }
        public IActionResult OnGet()
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (!bool.Parse(user.Split('|')[1]))
                return RedirectToPage(@"/Trainers/ReadAllTrainers");

            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            ITrainer trainer = Trainer;
            if (ModelState.IsValid && await _db.CreateTrainerAsync(trainer))
                return RedirectToPage(@"/Trainers/ReadAllTrainers");
            else
            {
                Message = "Træner blev ikke oprettet";
                return Page();
            }
        }
    }
}
