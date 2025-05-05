using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IEnumerable
{
    public class TeamDB_IENU : ITeamDB
    {

        public Task<bool> CreateTeamAsync(ITeam team)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTeamAsync(int teamID)
        {
            throw new NotImplementedException();
        }

        public Task <List<ITeam>> GetAllTeamAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ITeam?> GetTeamByIDAsync(int teamID)
        {
            throw new NotImplementedException();
        }

        public Task<ITeam> GetTeamsByMemberAsync(int memberID)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTeamCapacityAsync(int teamID)
        {
            throw new NotImplementedException();
        }

        public Task<List<ITeam>> GetTeamsByActiveDateAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<ITeam>> GetTeamsByTrainerAsync(string trainer)
        {
            throw new NotImplementedException();
        }

        public Task<List<ITeam>> SearchTeamsAsync(Dictionary<string, object> searchCriteria)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTeamAsync(ITeam team)
        {
            throw new NotImplementedException();
        }

        public Task<List<ITeam>> GetTeamsByActiveDayAsync(string day)
        {
            throw new NotImplementedException();
        }

        Task<List<ITeam>> ITeamDB.GetTeamsByMemberAsync(int memberID)
        {
            throw new NotImplementedException();
        }
    }
}
