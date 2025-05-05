using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IEnumerable
{
    public class TeamDB_IENU : ITeamDB
    {
        public bool CreateTeamASync(ITeam team)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTeamAsync(int teamID)
        {
            throw new NotImplementedException();
        }

        public List<ITeam> GetAllTeamAsync()
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllTrainersAsync()
        {
            throw new NotImplementedException();
        }

        public bool GetTeamByIDAsync(int teamID)
        {
            throw new NotImplementedException();
        }

        public ITeam GetTeamByMemberAsync(int memberID)
        {
            throw new NotImplementedException();
        }

        public int GetTeamCapacityAsync(int teamID)
        {
            throw new NotImplementedException();
        }

        public List<ITeam> GetTeamsByActiveDateAsync()
        {
            throw new NotImplementedException();
        }

        public List<ITeam> GetTeamsByTrainerAsync(string trainer)
        {
            throw new NotImplementedException();
        }

        public bool UpdateTeamAsync(ITeam team)
        {
            throw new NotImplementedException();
        }
    }
}
