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
        //private string insStr = @"INSERT INTO Members (Name, PassWord, Address, Email, Phone, Sex, DoB, Bio, IsAdmin, Image) VALUES (@Name, @Password, @Address, @Email, @Phone, @Sex, @Dob, @Bio, @IsAdmin, @Image)";
        //private string delStr = @"DELETE FROM Members WHERE ID = @ID";
        //private string updStr = @"UPDATE Members (Name, PassWord, Address, Email, Phone, Sex, DoB, Bio, IsAdmin, Image) SET (@Name, @Password, @Address, @Email, @Phone, @Sex, @Dob, @Bio, @IsAdmin, @Image) WHERE ID = @ID"

        public async Task<bool> CreateMemberAsync(IMember member)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                /*
                using (SqlCommand com = new SqlCommand(insStr, con))
                com.Parameters.AddWithValue("@Name", member.Name);
                com.Parameters.AddWithValue("@Password", member.Password);
                com.Parameters.AddWithValue("@Address", member.Address);
                com.Parameters.AddWithValue("@Email", member.Email);
                com.Parameters.AddWithValue("@Phone", member.Phone);
                com.Parameters.AddWithValue("@Sex", member.Sex);
                com.Parameters.AddWithValue("@Dob", string Bday = $"{member.Dob.Year}-{member.Dob.Month}-{member.Dob.Day}");
                com.Parameters.AddWithValue("@Bio", member.Bio);
                com.Parameters.AddWithValue("@IsAdmin", int isA = member.IsAdmin == false ? 0 : 1);
                com.Parameters.AddWithValue("@Image", string image = member.Image == null ? "null" : member.Image);
                 */

                string image = member.Image == null ? "null" : $"'{member.Image}'";
                string Bday = $"{member.Dob.Year}-{member.Dob.Month}-{member.Dob.Day}";
                int isA = member.IsAdmin == false ? 0 : 1;
                string insStr = $"INSERT INTO Members VALUES " +
                    $"('{member.Name}', '{member.Password}', '{member.Address}', '{member.Email}', " +
                    $"'{member.Phone}', '{member.Sex}', '{Bday}', '{member.Bio}', {isA}, {image});";
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
            /*
            queStr += @" WHERE ID = @ID";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand(queStr, con)
                com.Parameters.AddWithValue("@ID", memberID);
                return await SomethingQueryAsync(con, com);
            }
             */
        }

        public async Task<IMember> GetMemberByLoginAsync(string email, string password)
        {
            return (await GetMembersByQueryAsync(queStr + $" WHERE Email = '{email}' AND Password = '{password}'")).FirstOrDefault();
            /*
             queStr += @" WHERE Email = @EMAIL AND PassWord = @PASSWORD";
            using (SqlConnection con = new SqlConnection(connectionString)
            {   
                SqlCommand com = new SqlCommand(queStr, con)
                com.Parameters.AddWithValue("@EMAIL", email);
                com.Parameters.AddWithValue("@PASSWORD", password);
                return await SomethingQúeryAsync(con, com);
            }
             */
        }

        public async Task<List<IMember>> GetMembersByAdminAsync(bool isAdmin)
        {
            int isA = isAdmin == false ? 0 : 1;
            return await GetMembersByQueryAsync(queStr + $" WHERE IsAdmin = {isA}");
            /*
             queStr += @" WHERE IsAdmin = @ISADMIN";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand(queStr, con)
                com.Parameter.AddWithValue("@ISADMIN", isA);
                return await SomethingQueryAsync(con, com);
            }
             */
        }

        public async Task<List<IMember>> GetMembersByAgeAboveAsync(int age)
        {
            DateOnly actAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-age);
            return await GetMembersByQueryAsync(queStr + $" WHERE DoB < '{actAge}'");
            /*
            queStr += @"WHERE Dob < @AGETIME";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand(queStr, con)
                com.Parameter.AddWithValue("@AGETIME", actAge);
                return await SomethingQueryAsync(con, com);
            }
            */
        }

        public async Task<List<IMember>> GetMembersByAgeBelowAsync(int age)
        {
            DateOnly actAge = (DateOnly.FromDateTime(DateTime.Now)).AddYears(-age);
            return await GetMembersByQueryAsync(queStr + $" WHERE DoB > '{actAge}'");
            /*
            queStr += @"WHERE Dob > @AGETIME";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand(queStr, con)
                com.Parameter.AddWithValue("@AGETIME", actAge);
                return await SomethingQueryAsync(con, com);
            }
             */

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
            /*
              queStr += @"WHERE DoB BETWEEN @AGEFROM AND @AGETO"
              using (SqlConnection con = new SqlConnection(connectionString))
              {
                  SqlCommand com = new SqlCommand(queStr, con)
                  com.Parameter.AddWithValue("@AGEFROM", froAge);
                  com.Parameter.AddWithValue("@AGETO", toAge);
                  return await SomethingQueryAsync(con, com);
              }
            */
        }

        public async Task<List<IMember>> GetMembersBySexAsync(string sex)
        {
            return await GetMembersByQueryAsync(queStr + $" WHERE Sex = '{sex}'");
            /*
            queStr += @"WHERE Sex = @SEX";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand(queStr, con)
                com.Parameter.AddWithValue("@SEX", sex);)
                return await SomethingQueryAsync(con, com);
            }
            */
        }

        public async Task<bool> UpdateMemberAsync(IMember member)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                /*
                  
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                SqlCommand com = new SqlCommand(updStr, con);
                com.Parameters.AddWithValue("@Name", member.Name);
                com.Parameters.AddWithValue("@Password", member.Password);
                com.Parameters.AddWithValue("@Address", member.Address);
                com.Parameters.AddWithValue("@Email", member.Email);
                com.Parameters.AddWithValue("@Phone", member.Phone);
                com.Parameters.AddWithValue("@Sex", member.Sex);
                com.Parameters.AddWithValue("@Dob", string Bday = $"{member.Dob.Year}-{member.Dob.Month}-{member.Dob.Day}");
                com.Parameters.AddWithValue("@Bio", member.Bio);
                com.Parameters.AddWithValue("@IsAdmin", int isA = member.IsAdmin == false ? 0 : 1);
                com.Parameters.AddWithValue("@Image", string image = member.Image == null ? "null" : member.Image);
                com.Parameters.AddWithValue("@ID", member.Id);
                return await SomethingQueryAsync(con, com);
                }
                 */
                string image = member.Image == null ? "null" : $"'{member.Image}'";
                string Bday = $"{member.Dob.Year}-{member.Dob.Month}-{member.Dob.Day}";
                int isA = member.IsAdmin == false ? 0 : 1;
                string updStr = $"UPDATE Members SET " +
                    $"Name = '{member.Name}', PassWord = '{member.Password}', Address = '{member.Address}', Email = '{member.Email}', " +
                    $"Phone = '{member.Phone}', Sex = '{member.Sex}', DoB = '{Bday}', Bio = '{member.Bio}', Image = {image}, IsAdmin = {isA}" +
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
            /*
             int mID = rea.GetInt32(ID);
            string mNa = rea.GetString(Name);
            string mPa = rea.GetString(PassWord);
            string mAd = rea.GetString(Address);
            string mEm = rea.GetString(Email);
            string mPh = rea.GetString(Phone);
            string mSe = rea.GetString(Sex);
            DateOnly mDo = DateOnly.FromDateTime(rea.GetDateTime(DoB));
            string mBi = rea.GetString(Bio);
            bool mIa = rea.GetBoolean(IsAdmin); 
            string? mIm;
            if (!await rea.IsDBNullAsync(Image))
                mIm = rea.GetString(Image);
            else
                mIm = null;
             */
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
                SqlCommand queCon = new SqlCommand(que, con);

                await con.OpenAsync();
                int nOR = await queCon.ExecuteNonQueryAsync();
                return nOR > 0;
            }
        }

        private async Task<bool> SomethingQueryAsync(SqlConnection con, SqlCommand com)
        {
            await con.OpenAsync();
            int nOR = await com.ExecuteNonQueryAsync();
            return nOR > 0;
        }
    }
}
