using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Gadevang_Tennis_Klub.Pages.Courts
{
    public class GetAllCourtsModel : PageModel
    {
        public ICourtDB _cbd;
        public List<ICourt> Courts { get; set; }
        public bool IsAdmin;
        public GetAllCourtsModel(ICourtDB courtdb)
        {
            _cbd = courtdb;
        }
        private List<ICourt> SortByID(List<ICourt> listToSort)
        {
            List<ICourt> sortedList = listToSort;
            sortedList.OrderBy(court => court.ID);

            return sortedList;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Courts = await _cbd.GetAllCourtsAsync();
                SortByID(Courts);
                IsAdmin = false;
                string? user = HttpContext.Session.GetString("User");
                if (user == null)
                    return RedirectToPage(@"/User/Login");
                else if (user != null)
                    IsAdmin = bool.Parse(user.Split('|')[1]);

                return Page();
            }
            catch (Exception ex)
            {
                Courts = new List<ICourt>();
                ViewData["ErrorMessage"] = ex.Message;
            }
            return RedirectToPage("/Index");
        }
    }
}
