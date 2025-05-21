using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Services
{
    public interface IMemberDB
    {
        public Task<bool> CreateMemberAsync(IMember member);
        public Task<List<IMember>> GetAllMembersAsync();
        public Task<bool> DeleteMemberAsync(int memberID);
        public Task<bool> UpdateMemberAsync(IMember member);
        public Task<IMember> GetMemberByIDAsync(int memberID);
        public Task<List<IMember>> GetMembersBySexAsync(string sex);
        public Task<List<IMember>> GetMembersByAdminAsync(bool isAdmin);
        public Task<List<IMember>> GetMembersByAgeIntervalAsync(int from, int to);
        public Task<List<IMember>> GetMembersByAgeAboveAsync(int age);
        public Task<List<IMember>> GetMembersByAgeBelowAsync(int age);
        public Task<IMember> GetMemberByLoginAsync(string name, string password);
    }
}
