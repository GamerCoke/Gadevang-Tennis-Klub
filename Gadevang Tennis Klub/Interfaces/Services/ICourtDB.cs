using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface ICourtDB
    {
        public Task<bool> CreateCourtAsync(ICourt court);
        public Task<bool> UpdateCourtAsync(ICourt court);
        public Task<bool> DeleteCourtAsync(int courtID);
        public Task<ICourt> GetCourtByIDAsync(int courtID);
        public Task<List<ICourt>> GetAllCourtsAsync();
        public Task<List<ICourt>> GetCourtsByTypeAsync(string type);
        public Task<List<ICourt>> GetCourtsByNameAsync(string name);
    }
}
