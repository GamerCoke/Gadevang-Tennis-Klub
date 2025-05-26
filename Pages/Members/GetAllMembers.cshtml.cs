using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Members
{
    public class GetAllMembersModel : PageModel
    {
        public IMemberDB _memberDB;
        public ITeamDB _teamDB;
        public bool IsAdmin;
        public string? User;
        public int UserID;

        public List<IMember> Members { get; set; }

        public GetAllMembersModel(IMemberDB memberDB, ITeamDB teamDB)
        {
            _memberDB = memberDB;
            _teamDB = teamDB;
        }

        private List<IMember> SortByName(List<IMember> listToSort)
        {
            List<IMember> sortedList = listToSort;
            sortedList.Sort((d1, d2) => d1.Name.CompareTo(d2.Name));

            return sortedList;
        }

        public async Task OnGetAsync()
        {
            try
            {
                Members = await _memberDB.GetAllMembersAsync();
                SortByName(Members);
                User = HttpContext.Session.GetString("User");
                UserID = int.Parse(User.Split('|')[0]);
                if (User != null)
                    IsAdmin = bool.Parse(User.Split('|')[1]);
                else
                    IsAdmin = false;
            }
            catch (Exception ex)
            {
                Members = new List<IMember>();
                ViewData["ErrorMessage"] = ex.Message;
            }

        }
        public async Task<IActionResult> OnGetSearchAsync(string query)
        {
            var filtered = Members
                .Where(m => m.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(m => new { m.Name }) // Sender mindst muligt data tilbage
                .ToList();
            return new JsonResult(filtered);
        }
     
    }
            
}

