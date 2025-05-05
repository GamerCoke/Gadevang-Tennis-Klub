using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface ITrainerDB
    {
        Task<List<ITrainer>> GetAllTrainersAsync();
        Task<bool> CreateTrainerAsync(ITrainer trainer);
        Task<bool> DeleteTrainerAsync(int trainerID);
        Task<bool> UpdateTrainerAsync(ITrainer trainer);
        Task<ITrainer> GetTrainerByIDAsync(int trainerID);
    }
}
