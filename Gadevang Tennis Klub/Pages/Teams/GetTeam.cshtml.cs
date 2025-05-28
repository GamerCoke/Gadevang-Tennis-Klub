using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Teams
{
    public class GetTeamModel : PageModel
    {
        private ITeamDB _teamDB;
        private ITrainerDB _trainerDB;
        private ITeamBookingDB _teamBookingDB;

        public ITeam Team { get; set; }
        public ITrainer Trainer { get; set; }
        public List<ITeamBooking> TeamBookings { get; set; }


        public GetTeamModel(ITeamDB teamDB, ITrainerDB trainerDB, ITeamBookingDB teamBookingDB)
        {
            _teamDB = teamDB;
            _trainerDB = trainerDB;
            _teamBookingDB = teamBookingDB;
        }

        public async Task OnGetAsync(int teamID)
        {
            try
            {
                Team = await _teamDB.GetTeamByIDAsync(teamID);
                Trainer = await _trainerDB.GetTrainerByIDAsync(Team.TrainerId);
                TeamBookings = (await _teamBookingDB.GetAllTeamBookingAsync()).Where(teamBooking => teamBooking.Team_ID == Team.ID).ToList();
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
                Trainer = await _trainerDB.GetTrainerByIDAsync(Team.TrainerId);
                TeamBookings = (await _teamBookingDB.GetAllTeamBookingAsync()).Where(teamBooking => teamBooking.Team_ID == Team.ID).ToList();

                // Code to join team ??? 
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
