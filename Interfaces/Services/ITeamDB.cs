using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface ITeamDB
    {
        public List<ITeam> GetAllTeamAsync();
        public bool CreateTeamASync(ITeam team);
        public bool DeleteTeamAsync(int teamID);
        public bool UpdateTeamAsync(ITeam team);
        public bool GetTeamByIDAsync(int teamID);
        public int GetTeamCapacityAsync(int teamID);
        public List<ITeam> GetTeamsByTrainerAsync(string trainer);
        public List<string> GetAllTrainersAsync();
        public List<ITeam> GetTeamsByActiveDateAsync();
        public ITeam GetTeamByMemberAsync(int memberID);
    }
}
