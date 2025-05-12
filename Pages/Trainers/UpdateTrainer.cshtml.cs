using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Pages.Trainers
{
    public class UpdateTrainerModel : PageModel
    {

        [BindProperty]
        public Trainer Trainer { get; set; }
        [BindProperty] public IFormFile? Photo { get; set; }

        public string? Message;
        private ITrainerDB _db;
        private IWebHostEnvironment _webHostEnvironment;
        public UpdateTrainerModel(ITrainerDB db, IWebHostEnvironment webHostEnvironment)
        {
            Message = null;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> OnGet(int trainerId)
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (!bool.Parse(user.Split('|')[1]))
                return RedirectToPage(@"/Trainers/ReadAllTrainers");

            Trainer = (Trainer)await _db.GetTrainerByIDAsync(trainerId);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            Trainer.Image = ProcessUploadedFile();
            if (ModelState.IsValid && await _db.UpdateTrainerAsync(Trainer))
            {
                return RedirectToPage(@"/Trainers/ReadAllTrainers");
            }
            else
            {
                Message = "Træner blev ikke opdateret";
                return Page();
            }
        }
        private string? ProcessUploadedFile()
        {
            string? uniqueFileName = null;
            if (Photo != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                using (var fileStream = new FileStream(Path.Combine(Path.Combine(_webHostEnvironment.WebRootPath, @"Images"), uniqueFileName), FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
