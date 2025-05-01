using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IENumerable
{
    public class CourtDB_IENU : ICourtDB
    {
        public bool CreateCourtAsync(ICourt court)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCourtAsync(int courtID)
        {
            throw new NotImplementedException();
        }

        public List<ICourt> GetAllCourtsAsync()
        {
            throw new NotImplementedException();
        }

        public ICourt GetCourtByIDAsync(int courtID)
        {
            throw new NotImplementedException();
        }

        public List<ICourt> GetCourtsByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public List<ICourt> GetCourtsByTypeAsync(string type)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCourtAsync(ICourt court)
        {
            throw new NotImplementedException();
        }
    }
}
