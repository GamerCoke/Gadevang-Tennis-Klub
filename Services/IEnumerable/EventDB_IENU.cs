﻿using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IEnumerable
{
    public class EventDB_IENU : IEventDB
    {
        public Task<int?> CreateEventAsync(IEvent ev)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEventAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<List<IEvent>> GetAllEventsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEvent?> GetEventByIDAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<int?> GetEventCapacityAsync(int eventID)
        {
            throw new NotImplementedException();
        }

        public Task<List<IEvent>> GetEventsByDateAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<IEvent> SortEventsByDate(List<IEvent> listToSort)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEventAsync(IEvent ev)
        {
            throw new NotImplementedException();
        }
    }
}
