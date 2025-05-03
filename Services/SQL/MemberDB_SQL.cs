using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Numerics;

namespace Gadevang_Tennis_Klub.Services.SQL
{
    public class MemberDB_IENU : IMemberDB
    {
        private string connectionString = Secret.ConnectionString;
        private string queStr = "SELECT * FROM Members";

        public async Task<bool> CreateMemberAsync(IMember member)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string img = member.Image == null ? "null" : $"'{member.Image}'";
                int isA = member.IsAdmin ? 1 : 0;
                string insStr = $"INSERT INTO Members VALUES " +
                    $"('{member.Name}', '{member.Password}', '{member.Address}', '{member.Email}', " +
                    $"'{member.Phone}', '{member.Sex}', {member.Dob}, '{member.Bio}', {img}, {isA});";
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

        public async Task<IMember> GetMemberByLoginAsync(string name, string password)
        {
            return (await GetMembersByQueryAsync(queStr + $" WHERE Name = {name} AND Password = {password}")).FirstOrDefault();
        }

        public async Task<List<IMember>> GetMembersByAdminAsync(bool isAdmin)
        {
            return await GetMembersByQueryAsync(queStr + $" WHERE IsAdmin = {isAdmin}");
        }

        public async Task<List<IMember>> GetMembersByAgeAboveAsync(int age)
        {
            DateOnly actAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-age);
            return await GetMembersByQueryAsync(queStr + $" WHERE DoB < {actAge}");
        }

        public async Task<List<IMember>> GetMembersByAgeBelowAsync(int age)
        {
            DateOnly actAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-age);
            return await GetMembersByQueryAsync(queStr + $" WHERE DoB > {actAge}");
        }

        public async Task<List<IMember>> GetMembersByAgeIntervalAsync(int from, int to)
        {
            DateOnly froAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-from);
            DateOnly toAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-to);
            return await GetMembersByQueryAsync(queStr + $" WHERE DoB BETWEEN {froAge} AND {toAge}");
        }

        public async Task<List<IMember>> GetMembersBySexAsync(string sex)
        {
            return await GetMembersByQueryAsync(queStr + $" WHERE Sex = {sex}");
        }

        public async Task<bool> UpdateMemberAsync(IMember member)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string img = member.Image == null ? "null" : $"'{member.Image}'";
                int isA = member.IsAdmin ? 1 : 0;
                string updStr = $"UPDATE Members VALUES " +
                    $"('{member.Name}', '{member.Password}', '{member.Address}', '{member.Email}', " +
                    $"'{member.Phone}', '{member.Sex}', {member.Dob}, '{member.Bio}', {img}, {isA}" +
                    $"WHERE ID = {member.Id})";
                return await NonReadQueryAsync(updStr);
            }
        }

        private async Task<List<IMember>> GetMembersByQueryAsync(string que)
        {
            List<IMember> members = new();
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
            #region member DB ref
            //ID int identity(0, 1) UNIQUE,
            //Name varchar(256) not null,
            //PassWord varchar(256) not null,
            //Address varchar(256) not null,
            //Email varchar(256) not null,
            //Phone varchar(16) not null,
            //Sex varchar(8) not null,
            //DoB Date not null,
            //Bio varchar(1024) not null,
            //IsAdmin bit not null,
            //Image varchar(64)
            #endregion
            int mID = rea.GetInt32(0);
            string mNa = rea.GetString(1);
            string mPa = rea.GetString(2);
            string mAd = rea.GetString(3);
            string mEm = rea.GetString(4);
            string mPh = rea.GetString(5);
            string mSe = rea.GetString(6);
            DateOnly mDo = DateOnly.FromDateTime(rea.GetDateTime(7));
            string mBi = rea.GetString(8);
            bool mIa = rea.GetInt32(9) == 1;
            string? mIm = rea.GetString(10);
            #region member model ctor ref
            // int id,
            // string name,
            // string phone,
            // string email,
            // string? image,
            // string sex,
            // DateOnly doB,
            // bool isAdmin,
            // string bio,
            // string adress,
            // string password
            #endregion
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
