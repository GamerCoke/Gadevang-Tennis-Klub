using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IEnumerable
{
    public class EventDB_IENU : IEventDB
    {
        public bool CreateEventAsync(IEvent ev)
        {
            throw new NotImplementedException();
        }

        public bool DeleteEventAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public List<IEvent> GetAllEventsAsync()
        {
            throw new NotImplementedException();
        }

        public IEvent GetEventByIDAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public int GetEventCapacityAsync(int eventID)
        {
            throw new NotImplementedException();
        }

        public List<IEvent> GetEventsByDateAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<IEvent> GetEventsByMemberAsync(int memberID)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEventAsync(IEvent ev)
        {
            throw new NotImplementedException();
        }
    }
}
