using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Teams
{
    public class GetTeamModel : PageModel
    {
        private ITeamDB _teamDB;

        public ITeam Team { get; set; }


        public GetTeamModel(ITeamDB teamDB)
        {
            _teamDB = teamDB;
        }

        public async Task OnGetAsync(int teamID)
        {
            try
            {
                Team = await _teamDB.GetTeamByIDAsync(teamID);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }

        public async Task OnPostBookTeamAsync(int teamID)
        {
            try
            {
                Team = await _teamDB.GetTeamByIDAsync(teamID);

                // Code to join team ??? 
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
