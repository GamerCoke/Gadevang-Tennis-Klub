using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface IEventDB
    {
        List<IEvent> GetAllEventsAsync();
        bool CreateEventAsync(IEvent ev);
        bool DeleteEventAsync(int eventId);
        bool UpdateEventAsync(IEvent ev);
        IEvent GetEventByIDAsync(int eventId);
        List<IEvent> GetEventsByDateAsync(DateTime date);
        List<IEvent> GetEventsByMemberAsync(int memberID);
        int GetEventCapacityAsync(int eventID);
    }
}
