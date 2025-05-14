using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Courts
{
    public class GetCourtModel : PageModel
    {
        public ICourtDB _cbd;
        public Court Court;
        public bool IsAdmin;
        public GetCourtModel(ICourtDB courtdb)
        {
            _cbd = courtdb;
        }
        public async Task<IActionResult> OnGetAsync(int courtID)
        {
            try
            {
                Court = (Court)await _cbd.GetCourtByIDAsync(courtID);
                IsAdmin = false;
                string? user = HttpContext.Session.GetString("User");
                if (user != null)
                    IsAdmin = bool.Parse(user.Split('|')[1]);

                return Page();
            }
            catch (Exception ex)
            {
                Court = new Court();
                ViewData["ErrorMessage"] = ex.Message;
            }
            return RedirectToPage("/Index");
        }
    }
}
