using Microsoft.Data.SqlClient;
using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;

namespace Gadevang_Tennis_Klub.Services.SQL
{
    public class CourtDB_SQL : ICourtDB
    {
        private string connectionString = Secret.ConnectionString;
        private string queStr = "SELECT * FROM Courts";

        public async Task<bool> CreateCourtAsync(ICourt court)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                const string insStr = "INSERT INTO Courts VALUES (@TYPE, @NAME)";
                SqlCommand insQue = new SqlCommand(insStr, con);
                try
                {
                    insQue.Parameters.AddWithValue("@TYPE", court.Type);
                    insQue.Parameters.AddWithValue("@NAME", court.Name);
                    await con.OpenAsync();
                    int nOR = await insQue.ExecuteNonQueryAsync();
                    return nOR > 0;
                }
                catch (SqlException sqlExp)
                {
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return true;
        }

        public async Task<bool> DeleteCourtAsync(int courtID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string delStr = $"DELETE FROM Courts WHERE ID = {courtID}";
                SqlCommand delQue = new SqlCommand(delStr, con);

                await con.OpenAsync();
                int nOR = await delQue.ExecuteNonQueryAsync();
                return nOR > 0;
            }

        }

        public async Task<List<ICourt>> GetAllCourtsAsync()
        {
            List<ICourt> courts = new List<ICourt>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand com = new SqlCommand(queStr, con);
                    await com.Connection.OpenAsync();
                    SqlDataReader rea = await com.ExecuteReaderAsync();
                    while (await rea.ReadAsync())
                    {
                        int cID = rea.GetInt32(0);
                        string cTyp = rea.GetString(1);
                        string? cNam = rea.GetString(2);
                        Court court = new Court(cID, cTyp, cNam);
                        courts.Add(court);
                    }
                    rea.Close();
                }
                catch (SqlException sqlExp)
                {
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return courts;
        }

        public async Task<ICourt> GetCourtByIDAsync(int courtID)
        {
            List<ICourt> courts = new List<ICourt>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand com = new SqlCommand(queStr + $" WHERE ID = {courtID}", con);
                    await com.Connection.OpenAsync();
                    SqlDataReader rea = await com.ExecuteReaderAsync();
                    while (await rea.ReadAsync())
                    {
                        string cTyp = rea.GetString(1);
                        string? cNam = rea.GetString(2);
                        Court court = new Court(courtID, cTyp, cNam);
                        courts.Add(court);
                    }
                }
                catch (SqlException sqlExp)
                {
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return courts.First();
        }

        public async Task<List<ICourt>> GetCourtsByNameAsync(string name)
        {
            List<ICourt> courts = new List<ICourt>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand com = new SqlCommand(queStr + $" WHERE Name LIKE '%{name}%'", con);
                    await com.Connection.OpenAsync();
                    SqlDataReader rea = await com.ExecuteReaderAsync();
                    while (await rea.ReadAsync())
                    {
                        int cID = rea.GetInt32(0);
                        string cTyp = rea.GetString(1);
                        
                        Court court = new Court(cID, cTyp, name);
                        courts.Add(court);
                    }
                }
                catch (SqlException sqlExp)
                {
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return courts;
        }

        public async Task<List<ICourt>> GetCourtsByTypeAsync(string type)
        {
            List<ICourt> courts = new List<ICourt>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand com = new SqlCommand(queStr + $" WHERE Type = {type}", con);
                    await com.Connection.OpenAsync();
                    SqlDataReader rea = await com.ExecuteReaderAsync();
                    while (await rea.ReadAsync())
                    {
                        int cID = rea.GetInt32(0);
                        string? cNam = rea.GetString(2);

                        Court court = new Court(cID, type, cNam);
                        courts.Add(court);
                    }
                }
                catch (SqlException sqlExp)
                {
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return courts;
        }

        public async Task<bool> UpdateCourtAsync(ICourt court)
        {
            List<ICourt> courts = new List<ICourt>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string updStr = $"UPDATE Courts SET Type = {court.Type}, Name = {court.Name} WHERE ID = {court.ID}";
                SqlCommand updQue = new SqlCommand(updStr, con);
                try
                {
                    con.OpenAsync();
                    int nOR = await updQue.ExecuteNonQueryAsync();
                    return nOR > 0;
                }
                catch (SqlException sqlExp)
                {
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
