﻿using Microsoft.Data.SqlClient;
using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;

namespace Gadevang_Tennis_Klub.Services.SQL
{
    public class CourtDB_SQL : ICourtDB
    {
        private string connectionString = Secret.ConnectionString;
        private string queStr = "SELECT * FROM Courts";
        // private string insStr = @"INSTERT INTRO Courts (Type, Name) VALUES (@TYPE, @NAME)";
        // private string delStr = @"DELETE FROM Courts WHERE ID = @ID";
        // private string updStr = @"UPDATE Courts (Type, Name) SET (@TYPE, @NAME) WHERE ID = @ID";

        public async Task<bool> CreateCourtAsync(ICourt court)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // com.Parameters.AddWithValue(@TYPE, court.type);
                // com.Parameters.AddWithValue(@NAME, court.name);
                string insStr = $"INSERT INTO Courts VALUES ('{court.Type}', '{court.Name}');";
                return await NonReadQueryAsync(insStr);
            }
        }

        public async Task<bool> DeleteCourtAsync(int courtID)
        {
            return await NonReadQueryAsync($"DELETE FROM Courts WHERE ID = {courtID}");
        }

        public async Task<List<ICourt>> GetAllCourtsAsync()
        {
            return await GetCourtsByQueryAsync(queStr);
        }

        public async Task<ICourt> GetCourtByIDAsync(int courtID)
        {
            return (await GetCourtsByQueryAsync(queStr + $" WHERE ID = {courtID}")).FirstOrDefault();
        }

        public async Task<List<ICourt>> GetCourtsByNameAsync(string name)
        {
            return await GetCourtsByQueryAsync(queStr + $" WHERE Name LIKE '%{name}%'");
        }

        public async Task<List<ICourt>> GetCourtsByTypeAsync(string type)
        {
            return await GetCourtsByQueryAsync(queStr + $" WHERE Type LIKE '%{type}%'");
        }

        public async Task<bool> UpdateCourtAsync(ICourt court)
        {
            
            string name = court.Name != null ? $"\'{court.Name}\'" : "NULL";
            string updStr = $"UPDATE Courts SET Type = '{court.Type}', Name = {name} WHERE ID = {court.ID}";
            return await NonReadQueryAsync(updStr);
        }

        private async Task<ICourt> GetCourtByReaderAsync(SqlDataReader rea)
        {
            int cID = rea.GetInt32(0);
            string cTyp = rea.GetString(1);
            string? cNam = rea.GetString(2);
            Court court = new Court(cID, cTyp, cNam);
            return court;
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
        private async Task<List<ICourt>> GetCourtsByQueryAsync(string que)
        {
            List<ICourt> courts = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand(que, con);
                await com.Connection.OpenAsync();
                SqlDataReader rea = await com.ExecuteReaderAsync();

                while (await rea.ReadAsync())
                    courts.Add(await GetCourtByReaderAsync(rea));

                rea.Close();
            }
            return courts;
        }
    }
}
