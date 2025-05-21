using Gadevang_Tennis_Klub.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gadevang_Tennis_Klub.Models
{
    public class TeamBooking : ITeamBooking
    {
        [BindProperty]
        public int Member_ID { get; set; }
        [BindProperty]
        public int Team_ID { get ; set; }
        public int ID { get; set; }


        public TeamBooking()
        {
            Member_ID = 0;
            Team_ID = 0;
            ID = 0;
        }
        public TeamBooking(int Member_iD, int Team_iD, int iD) 
        {
            Member_ID = Member_iD;
            Team_ID = Team_iD;
            ID = iD;
        }
    }
}
