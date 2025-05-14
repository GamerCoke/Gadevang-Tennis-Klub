using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Courts
{
    public class CreateCourtModel : PageModel
    {
        public ICourtDB _cdb;
        public Court Court;
        public bool IsAdmin;

        public CreateCourtModel(ICourtDB courtdb)
        {
            _cdb = courtdb;
        }
        public IActionResult OnGet()
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (user != null)
                IsAdmin = bool.Parse(user.Split('|')[1]);

            return Page();
        }
    }
}
