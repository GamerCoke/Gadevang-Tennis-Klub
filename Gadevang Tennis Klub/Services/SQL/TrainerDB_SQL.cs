using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;

using Gadevang_Tennis_Klub.Models;
using Microsoft.Data.SqlClient;

namespace Gadevang_Tennis_Klub.Services.SQL
{
    public class TrainerDB_SQL : ITrainerDB
    {
        private string connectionString = Secret.ConnectionString;
        public async Task<bool> CreateTrainerAsync(ITrainer trainer)
        {
            return await NonReadQueryAsync($"insert into Trainers values ('{trainer.Name}', '{trainer.Phone}', '{trainer.Email}', null)");
        }

        public async Task<bool> DeleteTrainerAsync(int trainerID)
        {
            return await NonReadQueryAsync($"delete from Trainers where ID = {trainerID};");
        }

        public async Task<List<ITrainer>> GetAllTrainersAsync()
        {
            return await GetTrainerViaQueryAsync("select * from Trainers;");
        }

        public async Task<ITrainer> GetTrainerByIDAsync(int trainerID)
        {
            return (await GetTrainerViaQueryAsync($"select * from Trainers where ID = {trainerID};")).First();
        }

        public async Task<bool> UpdateTrainerAsync(ITrainer trainer)
        {
            string image = trainer.Image == null ? "null" : $"'{trainer.Image}'";
            return await NonReadQueryAsync(
                $"update Trainers " +
                $"set " +
                $"Name = '{trainer.Name}', " +
                $"Phone = '{trainer.Phone}', " +
                $"Email = '{trainer.Email}', " +
                $"Image = {image} " +
                $"where ID = {trainer.Id};");
        }

        private async Task<List<ITrainer>> GetTrainerViaQueryAsync(string query)
        {
            List<ITrainer> trainers = new();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    trainers.Add(await GetTrainerFromReaderAsync(reader));
                }
                reader.Close();
            }
            return trainers;
        }
        private async Task<bool> NonReadQueryAsync(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand commandQuery = new SqlCommand(query, connection);
                connection.Open();
                int noOfRows = await commandQuery.ExecuteNonQueryAsync();
                return noOfRows > 0;
            }
        }
        private async Task<ITrainer> GetTrainerFromReaderAsync(SqlDataReader reader)
        {
            int ID = reader.GetInt32(0);
            string name = reader.GetString(1);
            string phone = reader.GetString(2);
            string email = reader.GetString(3);

            string? image;
            if (!await reader.IsDBNullAsync(4))
                image = reader.GetString(4);
            else
                image = null;

            ITrainer trainer = new Trainer(ID, name, phone, email, image);

            return trainer;
        }

        public async Task<ITrainer?> GetTrainerByTeamIDAsync(int teamID)
        {
            string query = @"
        SELECT m.ID, m.Name, m.Phone, m.Email, m.Image
        FROM Teams t
        INNER JOIN Members m ON t.TrainerID = m.ID
        WHERE t.ID = @TeamID";

            using SqlConnection connection = new SqlConnection(Secret.ConnectionString);
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@TeamID", teamID);

            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new Trainer
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Phone = reader.GetString(2),
                        Email = reader.GetString(3),
                        Image = reader.IsDBNull(4) ? null : reader.GetString(4)
                    };
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error (GetTrainerByTeamIDAsync): " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error (GetTrainerByTeamIDAsync): " + ex.Message);
            }

            return null;
        }

    }
}
