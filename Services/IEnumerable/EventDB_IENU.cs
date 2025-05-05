using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IENumerable
{
    public class EventDB_IENU : IEventDB
    {
        public async Task<bool> CreateEventAsync(IEvent ev)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IEvent>> GetAllEventsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEvent?> GetEventByIDAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public async Task<int?> GetEventCapacityAsync(int eventID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IEvent>> GetEventsByDateAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateEventAsync(IEvent ev)
        {
            throw new NotImplementedException();
        }
    }
}
