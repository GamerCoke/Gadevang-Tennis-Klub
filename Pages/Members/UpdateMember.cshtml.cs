using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Members
{
    public class UpdateMemberModel : PageModel
    {
        public bool IsAdmin;

        public IActionResult OnGet(int memberID)
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (!(bool.Parse(user.Split('|')[1]) || memberID == int.Parse(user.Split('|')[0])))
                return RedirectToPage(@"/Members/GetAllMembers");
            else
                return Page();

            
        }
    }
}
