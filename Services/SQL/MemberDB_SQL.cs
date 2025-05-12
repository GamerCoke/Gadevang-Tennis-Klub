using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Numerics;
using System.Reflection.PortableExecutable;
using Microsoft.IdentityModel.Tokens;

namespace Gadevang_Tennis_Klub.Services.SQL
{
    public class MemberDB_SQL : IMemberDB
    {
        private string connectionString = Secret.ConnectionString;
        private string queStr = "SELECT * FROM Members";

        public async Task<bool> CreateMemberAsync(IMember member)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (member.Image.IsNullOrEmpty())
                    member.Image = "null";
                if (!member.Image.IsNullOrEmpty())
                    member.Image = "\'" + $"{member.Image}" + "\'";
                int year = member.Dob.Year;
                int month = member.Dob.Month;
                int day = member.Dob.Day;
                int isA = member.IsAdmin == false ? 0 : 1;
                string insStr = $"INSERT INTO Members VALUES " +
                    $"('{member.Name}', '{member.Password}', '{member.Address}', '{member.Email}', " +
                    $"'{member.Phone}', '{member.Sex}', '{year}-{month}-{day}', '{member.Bio}', {isA}, {member.Image});";
                return await NonReadQueryAsync(insStr);
            }
        }

        public async Task<bool> DeleteMemberAsync(int memberID)
        {
            return await NonReadQueryAsync($"DELETE FROM Members WHERE ID = {memberID}");
        }

        public async Task<List<IMember>> GetAllMembersAsync()
        {

            return await GetMembersByQueryAsync(queStr);

        }

        public async Task<IMember> GetMemberByIDAsync(int memberID)
        {
            return (await GetMembersByQueryAsync(queStr + $" WHERE ID = {memberID}")).FirstOrDefault();
        }

        public async Task<IMember> GetMemberByLoginAsync(string email, string password)
        {
            return (await GetMembersByQueryAsync(queStr + $" WHERE Email = '{email}' AND Password = '{password}'")).FirstOrDefault();
        }

        public async Task<List<IMember>> GetMembersByAdminAsync(bool isAdmin)
        {
            int isA = isAdmin == false ? 0 : 1;
            return await GetMembersByQueryAsync(queStr + $" WHERE IsAdmin = {isA}");
        }

        public async Task<List<IMember>> GetMembersByAgeAboveAsync(int age)
        {
            DateOnly actAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-age);
            return await GetMembersByQueryAsync(queStr + $" WHERE DoB < '{actAge}'");
        }

        public async Task<List<IMember>> GetMembersByAgeBelowAsync(int age)
        {
            DateOnly actAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-age);
            return await GetMembersByQueryAsync(queStr + $" WHERE DoB > '{actAge}'");
        }

        public async Task<List<IMember>> GetMembersByAgeIntervalAsync(int from, int to)
        {
            if (from > to) 
            {
                DateOnly froAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-from);
                DateOnly toAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-to);
                return await GetMembersByQueryAsync(queStr + $" WHERE DoB BETWEEN '{froAge}' AND '{toAge}'");
            }
            else 
            {
                DateOnly froAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-to);
                DateOnly toAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-from);
                return await GetMembersByQueryAsync(queStr + $" WHERE DoB BETWEEN '{froAge}' AND '{toAge}'");
            }
        }

        public async Task<List<IMember>> GetMembersBySexAsync(string sex)
        {
            return await GetMembersByQueryAsync(queStr + $" WHERE Sex = '{sex}'");
        }

        public async Task<bool> UpdateMemberAsync(IMember member)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string img = member.Image == null ? "null" : $"'{member.Image}'";
                string updStr = $"UPDATE Members SET " +
                    $"(Name = '{member.Name}', PassWord = '{member.Password}', Address = '{member.Address}', Email = '{member.Email}', " +
                    $"Phone = '{member.Phone}', Sex = '{member.Sex}', DoB = {member.Dob}, Bio = '{member.Bio}', Image = {img}, IsAdmin = {member.IsAdmin})" +
                    $" WHERE ID = {member.Id}";
                return await NonReadQueryAsync(updStr);
            }
        }

        private async Task<List<IMember>> GetMembersByQueryAsync(string que)
        {
            List<IMember> members = new List<IMember>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand(que, con);
                await com.Connection.OpenAsync();
                SqlDataReader rea = await com.ExecuteReaderAsync();

                while (await rea.ReadAsync())
                    members.Add(await GetMemberByReaderAsync(rea));

                rea.Close();
            }
            return members;
        }
        private async Task<IMember> GetMemberByReaderAsync(SqlDataReader rea)
        {
            int mID = rea.GetInt32(0);
            string mNa = rea.GetString(1);
            string mPa = rea.GetString(2);
            string mAd = rea.GetString(3);
            string mEm = rea.GetString(4);
            string mPh = rea.GetString(5);
            string mSe = rea.GetString(6);
            DateOnly mDo = DateOnly.FromDateTime(rea.GetDateTime(7));
            string mBi = rea.GetString(8);
            bool mIa = rea.GetBoolean(9); 
            string? mIm;
            if (!await rea.IsDBNullAsync(10))
                mIm = rea.GetString(10);
            else
                mIm = null;

            Member member = new Member(mID, mNa, mPh, mEm, mIm, mSe, mDo, mIa, mBi, mAd, mPa);
            return member;
        }

        private async Task<bool> NonReadQueryAsync(string que)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand delQue = new SqlCommand(que, con);

                await con.OpenAsync();
                int nOR = await delQue.ExecuteNonQueryAsync();
                return nOR > 0;
            }
        }
    }
}
