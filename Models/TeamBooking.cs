using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Models
{
    public class TeamBooking : ITeamBooking
    {
        public int Member_ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Team_ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public TeamBooking(int Member_iD, int Team_iD, int iD) 
        {
            Member_ID = Member_iD;
            Team_ID = Team_iD;
            ID = iD;
        }
    }
}
