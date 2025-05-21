using Microsoft.AspNetCore.Mvc.RazorPages;
using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Pages.Teams
{
    public class GetAllTeamsModel : PageModel
    {
        private readonly ITeamDB _teamDB;

        public List<ITeam> Teams { get; set; }

        public GetAllTeamsModel(ITeamDB teamDB)
        {
            _teamDB = teamDB;
            Teams = new List<ITeam>();
        }

        public async Task OnGetAsync()
        {
            Teams = await _teamDB.GetAllTeamsAsync();
        }
    }
}
