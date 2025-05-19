using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Pages.User
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Kodeord { get; set; }
        public IMemberDB MemberDB { get; set; }
        public string? UserCookie { get; set; }
        public string? Message { get; set; }

        public LoginModel(IMemberDB memberDB)
        {
            UserCookie = null;
            MemberDB = memberDB;
        }

        public void OnGet()
        {
            UserCookie = HttpContext.Session.GetString("User");
        }

        public IActionResult OnPostLogin()
        {
            IMember user = MemberDB.GetMemberByLoginAsync(Email, Kodeord).Result; if (user != null)
            {
                HttpContext.Session.SetString("User", $"{user.Id}|{user.IsAdmin}|{user.Name}");
                return RedirectToPage(@"MyPage");
            }
            else
            {
                Message = $"Email: {Email} og Kodeord: {Kodeord} er ikke en genkendt kombination i vores register";
                return Page();
            }
        }
    }
}
