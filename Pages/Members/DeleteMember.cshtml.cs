using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Gadevang_Tennis_Klub.Pages.Members
{
    public class DeleteMemberModel : PageModel
    {
        [BindProperty]
        public IMember? Member { get; set; }

        [BindProperty]
        public IFormFile? Photo { get; set; }

        public IMemberDB _memberDB;
        public bool IsAdmin;
        public DeleteMemberModel(IMemberDB memberDB)
        {
            _memberDB = memberDB;
        }

        public async Task<IActionResult> OnGetAsync(int memberID)
        {
            IsAdmin = false;
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (user != null)
                IsAdmin = bool.Parse(user.Split('|')[1]);

            if (!(IsAdmin || memberID == int.Parse(user.Split('|')[0])))
                return RedirectToPage(@"/Members/GetAllMembers");

             try
                {
                Member = await _memberDB.GetMemberByIDAsync(memberID);
                    return Page();
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                }
            return RedirectToPage(@"/Index");
        }

        public async Task<IActionResult> OnPost(int memberID)
        {
            try
            {
                IsDeleted = await _memberDB.DeleteMemberAsync(memberID);
                return RedirectToPage(@"/Members/GetAllMembers");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
        }
    }
}
