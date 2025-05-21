using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.User
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("User") == null)
                return RedirectToPage(@"Login");
            else
                return Page();
        }
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToPage(@"Login");
        }
    }
}
