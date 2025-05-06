using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Gadevang_Tennis_Klub.Services.SQL
{
    public class TeamDB_SQL : Secret, ITeamDB
    {
        private string insertSQL = @"INSERT INTO Teams Values(@Name, @Description, @TrainerID, @Capacity, @ActiveDay, @Price)";
        private string queryString = @"SELECT ID, Name, Description, TrainerID, Capacity, ActiveDay, Price FROM Teams";
        private string updateString = @"UPDATE Teams SET Name = @Name, Description = @Description, TrainerID = @TrainerID, Capacity = @Capacity, ActiveDay = @ActiveDay, Price = @Price  WHERE ID = @ID";


        public async Task<bool> CreateTeamAsync(ITeam team)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(insertSQL, connection);
                    cmd.Parameters.AddWithValue("@Name", team.Name);
                    cmd.Parameters.AddWithValue("@Description", team.Description);
                    cmd.Parameters.AddWithValue("@TrainerID", team.Trainer?.Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Capacity", team.Capacity);
                    cmd.Parameters.AddWithValue("@ActiveDay", team.ActiveDay);
                    cmd.Parameters.AddWithValue("@Price", team.Price);

                    Console.WriteLine("Opening database connection...");
                    await connection.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    Console.WriteLine($"Antal indsatte i tabellen {rowsAffected}");

                    return rowsAffected > 0;
                }
                catch (SqlException sqlx)
                {
                    Console.WriteLine(sqlx.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<bool> DeleteTeamAsync(int teamID)
        {
            string deleteSQL = @"DELETE FROM Teams WHERE ID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(deleteSQL, connection))
            {
                cmd.Parameters.AddWithValue("@ID", teamID);

                try
                {
                    await connection.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error while deleting team: " + ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General Error while deleting team: " + ex.Message);
                    return false;
                }
            }
        }


        public async Task<List<ITeam>> GetAllTeamAsync()
        {
            List<ITeam> teams = new List<ITeam>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(queryString, connection);
                    await cmd.Connection.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        ITeam team = new Team(); // Initialize team here
                        team.ID = Convert.ToInt32(reader["ID"]);
                        team.Name = reader["Name"].ToString();
                        team.Description = reader["Description"].ToString();
                        int trainerID = Convert.ToInt32(reader["TrainerID"]);
                        team.Capacity = Convert.ToInt32(reader["Capacity"]);
                        team.Price = Convert.ToInt32(reader["Price"]);
                        team.ActiveDay = reader["ActiveDay"].ToString();

                        teams.Add(team);
                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                }
                finally
                {

                }
            }
            return teams;

        }

        public async Task<ITeam?> GetTeamByIDAsync(int teamID)
        {
            var criteria = new Dictionary<string, object> { { "ID", teamID } };
            var result = await SearchTeamsAsync(criteria);
            return result.FirstOrDefault();
        }

        public async Task<List<ITeam>> GetTeamsByMemberAsync(int memberID)
        {
            List<object> teamIds = new List<object>();

            string query = @"SELECT TeamID FROM TeamBookings WHERE MemberID = @MemberID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@MemberID", memberID);

                try
                {
                    await connection.OpenAsync();
                    using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        teamIds.Add(reader.GetInt32(0));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching TeamIDs: " + ex.Message);
                    return new List<ITeam>();
                }
            }

            if (teamIds.Count == 0)
                return new List<ITeam>();

            var criteria = new Dictionary<string, object>
            {
                { "ID", teamIds }
            };

            return await SearchTeamsAsync(criteria);
        }


        public async Task<int> GetTeamCapacityAsync(int teamID)
        {
            var criteria = new Dictionary<string, object> { { "ID", teamID } };
            var result = await SearchTeamsAsync(criteria);

            if (result.FirstOrDefault() is ITeam team)
            {
                return team.Capacity;
            }

            return 0;
        }


        public async Task<List<ITeam>> GetTeamsByActiveDayAsync(string day)
        {
            var criteria = new Dictionary<string, object> { { "ActiveDay", day } };
            return await SearchTeamsAsync(criteria);
        }


        public async Task<List<ITeam>> GetTeamsByTrainerAsync(string trainerId)
        {
            var criteria = new Dictionary<string, object> { { "TrainerID", trainerId } };
            return await SearchTeamsAsync(criteria);
        }


        public async Task<List<ITeam>> SearchTeamsAsync(Dictionary<string, object> searchCriteria)
        {
            List<ITeam> teams = new List<ITeam>();

            string baseQuery = @"SELECT ID, Name, Description, TrainerID, Capacity, ActiveDay, Price FROM Teams";
            var whereClauses = new List<string>();
            var parameters = new List<SqlParameter>();

            int paramIndex = 0;
            foreach (var criterion in searchCriteria)
            {
                if (criterion.Value is IEnumerable<object> values && !(criterion.Value is string))
                {
                    // IN clause for values
                    var valueList = values.Cast<object>().ToList();
                    if (valueList.Count == 0)
                        continue;

                    var paramNames = new List<string>();
                    for (int i = 0; i < valueList.Count; i++)
                    {
                        string paramName = $"@param{paramIndex}";
                        paramNames.Add(paramName);
                        parameters.Add(new SqlParameter(paramName, valueList[i] ?? DBNull.Value));
                        paramIndex++;
                    }

                    whereClauses.Add($"{criterion.Key} IN ({string.Join(", ", paramNames)})");
                }
                else
                {
                    string paramName = $"@param{paramIndex}";
                    whereClauses.Add($"{criterion.Key} = {paramName}");
                    parameters.Add(new SqlParameter(paramName, criterion.Value ?? DBNull.Value));
                    paramIndex++;
                }
            }

            string finalQuery = baseQuery;
            if (whereClauses.Any())
            {
                finalQuery += " WHERE " + string.Join(" AND ", whereClauses);
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(finalQuery, connection))
            {
                cmd.Parameters.AddRange(parameters.ToArray());

                try
                {
                    await connection.OpenAsync();
                    using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var team = new Team
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Capacity = Convert.ToInt32(reader["Capacity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            ActiveDay = reader["ActiveDay"].ToString(),
                            Trainer = new Trainer { Id = Convert.ToInt32(reader["TrainerID"]) }
                        };

                        teams.Add(team);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in SearchTeamsAsync: " + ex.Message);
                }
            }

            return teams;
        }

        public async Task<bool> UpdateTeamAsync(ITeam team)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(updateString, connection))
            {
                cmd.Parameters.AddWithValue("@ID", team.ID);
                cmd.Parameters.AddWithValue("@Name", team.Name);
                cmd.Parameters.AddWithValue("@Description", team.Description);
                cmd.Parameters.AddWithValue("@TrainerID", team.Trainer?.Id ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Capacity", team.Capacity);
                cmd.Parameters.AddWithValue("@ActiveDay", team.ActiveDay);
                cmd.Parameters.AddWithValue("@Price", team.Price);

                try
                {
                    await connection.OpenAsync();
                    int affectedRows = await cmd.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating team: " + ex.Message);
                    return false;
                }
            }
        }

    }
}
