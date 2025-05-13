using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Courts
{
    public class UpdateCourtModel : PageModel
    {
        public ICourtDB _cbd;
        public Court Court;
        public bool IsAdmin;
        public UpdateCourtModel(ICourtDB courtdb)
        {
            _cbd = courtdb;
        }
        public IActionResult OnGet()
        {
            IsAdmin = false;
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (user != null)
                IsAdmin = bool.Parse(user.Split('|')[1]);

            return Page();
        }
    }
}
