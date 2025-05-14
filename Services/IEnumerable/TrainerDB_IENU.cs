using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IEnumerable
{
    public class TrainerDB_IENU : ITrainerDB
    {
        public Task<bool> CreateTrainerAsync(ITrainer trainer)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTrainerAsync(int trainerID)
        {
            throw new NotImplementedException();
        }

        public Task<List<ITrainer>> GetAllTrainersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ITrainer> GetTrainerByIDAsync(int trainerID)
        {
            throw new NotImplementedException();
        }

        public Task<ITrainer> GetTrainerByTeamIDAsync(int teamID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTrainerAsync(ITrainer trainer)
        {
            throw new NotImplementedException();
        }
    }
}
