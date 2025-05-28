using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface IAnnouncementDB
    {
        public Task<bool> CreateAnnouncementAsync(IAnnouncement announcement);
        public Task<List<IAnnouncement>> GetAllAnnouncementsAsync();
        public Task<List<IAnnouncement>> SearchAnnouncementsAsync(Dictionary<string, (string Operator, object Value)> searchCriteria);
        public Task<IAnnouncement?> GetAnnouncementByIDAsync(int announcementID);
        public Task<List<IAnnouncement>> GetAnnouncementsByDateIntervalAsync(DateTime date1, DateTime date2);
        public Task<List<IAnnouncement>> GetAnnouncementsByDateOlderThanAsync(DateTime date);
        public Task<List<IAnnouncement>> GetAnnouncementsAfterDateAsync(DateTime date);
        public Task<List<IAnnouncement>> UpdateAnnouncementAsync(IAnnouncement announcement);
        public Task<bool> DeleteAnnouncementAsync(int announcementID);
        public Task<List<IAnnouncement>> GetAnnouncementsByTypeAsync(string type);
    }
}
