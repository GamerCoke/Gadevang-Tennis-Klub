using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Courts
{
    public class CreateCourtModel : PageModel
    {
        private ICourtDB _cdb;

        [BindProperty]
        public Court Court { get; set; }
        public string? Message;

        public CreateCourtModel(ICourtDB courtdb)
        {
            Message = null;
            _cdb = courtdb;
        }
        public IActionResult OnGet()
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (!bool.Parse(user.Split('|')[1]))
                return RedirectToPage(@"/Courts/GetAllCourts");

            return Page();
        }

        public async Task<IActionResult> OnPostCreate()
        {
            ICourt court = Court;
            if (ModelState.IsValid && await _cdb.CreateCourtAsync(court))
                return RedirectToPage(@"/Courts/GetAllCourts");
            else
            {
                Message = "Medlem blev ikke oprettet";
                return Page();
            }
        }
    }
}
