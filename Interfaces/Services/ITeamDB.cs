using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface ITeamDB
    {
        public Task <List<ITeam>> GetAllTeamAsync();
        public Task <bool> CreateTeamAsync(ITeam team);
        public Task <bool> DeleteTeamAsync(int teamID);
        public Task<bool> UpdateTeamAsync(ITeam team);
        public Task<ITeam?> GetTeamByIDAsync(int teamID);
        public Task <int> GetTeamCapacityAsync(int teamID);
        public Task <List<ITeam>> GetTeamsByTrainerAsync(string trainer);
        public Task<List<ITeam>> GetTeamsByActiveDayAsync(string day);
        public Task<List<ITeam>> GetTeamsByMemberAsync(int memberID);
        public Task<List<ITeam>> SearchTeamsAsync(Dictionary<string, object> searchCriteria);

    }
}
