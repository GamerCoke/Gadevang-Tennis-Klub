using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Courts
{
    public class DeleteCourtModel : PageModel
    {
        private ICourtDB _cbd;

        [BindProperty]
        public Court Court { get; set; }
        public DeleteCourtModel(ICourtDB courtdb)
        {
            _cbd = courtdb;
        }

        public async  Task<IActionResult> OnGetAsync(int courtID)
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (!bool.Parse(user.Split('|')[1]))
                return RedirectToPage(@"/Courts/GetAllCourts");

            try
            {
                Court = (Court)await _cbd.GetCourtByIDAsync(courtID);
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return RedirectToPage(@"/Index");
        }

        public async Task<IActionResult> OnPost(int courtID)
        {
            try
            {
                await _cbd.DeleteCourtAsync(courtID);
                return RedirectToPage(@"/Courts/GetAllCourts");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                Court = (Court)await _cbd.GetCourtByIDAsync(courtID);
                return Page();
            }
        }
    }
}
