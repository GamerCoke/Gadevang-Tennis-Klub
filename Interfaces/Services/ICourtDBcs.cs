using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface ICourtDBcs
    {
        public bool CreateCourtAsync(ICourt court);
        public bool UpdateCourtAsync(ICourt court);
        public bool DeleteCourtAsync(int courtID);
        public ICourt GetCourtByIDAsync(int courtID);
        public List<ICourt> GetAllCourtsAsync();
        public List<ICourt> GetCourtsByTypeAsync(string type);
    }
}
