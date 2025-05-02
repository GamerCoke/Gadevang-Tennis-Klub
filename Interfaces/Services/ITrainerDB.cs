using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface ITrainerDB
    {
        public Task<List<ITrainer>> GetAllTrainersAsync();
        public Task<bool> CreateTrainerAsync(ITrainer trainer);
        public Task<bool> DeleteTrainerAsync(int trainerID);
        public Task<bool> UpdateTrainerAsync(ITrainer trainer);
        public Task<ITrainer> GetTrainerByIDAsync(int trainerID);
    }
}
