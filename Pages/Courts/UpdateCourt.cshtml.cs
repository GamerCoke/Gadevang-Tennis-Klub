using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Courts
{
    public class UpdateCourtModel : PageModel
    {
        private ICourtDB _cbd;
        [BindProperty]
        public Court Court { get; set; }
        public bool IsAdmin;
        public string? Message;
        public UpdateCourtModel(ICourtDB courtdb)
        {
            Message = null;
            _cbd = courtdb;
        }
        public async Task<IActionResult> OnGetAsync(int courtID)
        {
            IsAdmin = false;
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (user != null)
                IsAdmin = bool.Parse(user.Split('|')[1]);

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
                if (ModelState.IsValid && await _cbd.UpdateCourtAsync(Court))
                    return RedirectToPage(@"/Courts/GetAllCourts");
                else
                {
                    Message = "Medlem blev ikke oprettet";
                    return Page();
                }
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
