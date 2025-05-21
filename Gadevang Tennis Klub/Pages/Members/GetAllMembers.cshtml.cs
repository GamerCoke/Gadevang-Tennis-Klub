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
                string? user = HttpContext.Session.GetString("User");
                if (user != null)
                    IsAdmin = bool.Parse(user.Split('|')[1]);
                else
                    IsAdmin = false;
            }
            catch (Exception ex)
            {
                Members = new List<IMember>();
                ViewData["ErrorMessage"] = ex.Message;
            }

        }
     
    }
            
}

