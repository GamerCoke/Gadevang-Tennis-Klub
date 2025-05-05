using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

namespace Gadevang_Tennis_Klub.Services.IEnumerable
{
    public class MemberDB_IENU : IMemberDB
    {
        public Task<bool> CreateMemberAsync(IMember member)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMemberAsync(int memberD)
        {
            throw new NotImplementedException();
        }

        public Task<List<IMember>> GetAllMembersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IMember> GetMemberByIDAsync(int memberID)
        {
            throw new NotImplementedException();
        }

        public Task<IMember> GetMemberByLoginAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<IMember>> GetMembersByAdminAsync(bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<List<IMember>> GetMembersByAgeAboveAsync(int age)
        {
            throw new NotImplementedException();
        }

        public Task<List<IMember>> GetMembersByAgeBelowAsync(int age)
        {
            throw new NotImplementedException();
        }

        public Task<List<IMember>> GetMembersByAgeIntervalAsync(int from, int to)
        {
            throw new NotImplementedException();
        }

        public Task<List<IMember>> GetMembersBySexAsync(string sex)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMemberAsync(IMember member)
        {
            throw new NotImplementedException();
        }
    }
}
