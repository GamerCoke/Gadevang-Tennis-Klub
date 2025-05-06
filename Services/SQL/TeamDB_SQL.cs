using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Gadevang_Tennis_Klub.Services;
using Microsoft.Data.SqlClient;

public class TeamDB_SQL : ITeamDB
{
    private string _connectionString = Secret.ConnectionString;
    private string insertString = @"INSERT INTO Teams VALUES(@Name, @Description, @TrainerID, @Capacity, @ActiveDay, @Price)";
    private string queryString = @"SELECT ID, Name, Description, TrainerID, Capacity, ActiveDay, Price FROM Teams";
    private string updateString = @"UPDATE Teams SET Name = @Name, Description = @Description, TrainerID = @TrainerID, Capacity = @Capacity, ActiveDay = @ActiveDay, Price = @Price WHERE ID = @ID";

    public async Task<bool> CreateTeamAsync(ITeam team)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        try
        {
            SqlCommand cmd = new SqlCommand(insertString, connection);
            cmd.Parameters.AddWithValue("@Name", team.Name);
            cmd.Parameters.AddWithValue("@Description", team.Description);
            cmd.Parameters.AddWithValue("@TrainerID", team.Trainer?.Id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Capacity", team.Capacity);
            cmd.Parameters.AddWithValue("@ActiveDay", team.ActiveDay);
            cmd.Parameters.AddWithValue("@Price", team.Price);

            await connection.OpenAsync();
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        catch (SqlException sqlEx)
        {
            Console.WriteLine("SQL Error (CreateTeamAsync): " + sqlEx.Message);
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("General Error (CreateTeamAsync): " + ex.Message);
            return false;
        }
        finally
        {
            connection.Close();
        }
    }

    public async Task<bool> DeleteTeamAsync(int teamID)
    {
        string deleteSQL = @"DELETE FROM Teams WHERE ID = @ID";
        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(deleteSQL, connection);
        cmd.Parameters.AddWithValue("@ID", teamID);

        try
        {
            await connection.OpenAsync();
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        catch (SqlException sqlEx)
        {
            Console.WriteLine("SQL Error (DeleteTeamAsync): " + sqlEx.Message);
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("General Error (DeleteTeamAsync): " + ex.Message);
            return false;
        }
        finally
        {
            connection.Close();
        }
    }

    public async Task<List<ITeam>> GetAllTeamAsync()
    {
        List<ITeam> teams = new();
        using SqlConnection connection = new SqlConnection(_connectionString);
        try
        {
            SqlCommand cmd = new SqlCommand(queryString, connection);
            await connection.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                ITeam team = new Team
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

            reader.Close();
        }
        catch (SqlException sqlEx)
        {
            Console.WriteLine("SQL Error (GetAllTeamAsync): " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("General Error (GetAllTeamAsync): " + ex.Message);
        }
        finally
        {
            connection.Close();
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
        List<int> teamIds = new();
        string query = @"SELECT TeamID FROM TeamBookings WHERE MemberID = @MemberID";

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(query, connection);
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
        catch (SqlException sqlEx)
        {
            Console.WriteLine("SQL Error (GetTeamsByMemberAsync): " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("General Error (GetTeamsByMemberAsync): " + ex.Message);
        }
        finally
        {
            connection.Close();
        }

        // If no teams found, return an empty list
        if (teamIds.Count == 0)
            return new List<ITeam>();

        // Use SearchTeamsAsync with the list of Team IDs
        var criteria = new Dictionary<string, object> { { "ID", teamIds } };
        return await SearchTeamsAsync(criteria);
    }


    public async Task<int> GetTeamCapacityAsync(int teamID)
    {
        var criteria = new Dictionary<string, object> { { "ID", teamID } };
        var result = await SearchTeamsAsync(criteria);
        return result.FirstOrDefault()?.Capacity ?? 0;
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
        List<ITeam> teams = new();
        List<string> whereClauses = new();
        List<SqlParameter> parameters = new();
        int paramCounter = 0;

        foreach (var criterion in searchCriteria)
        {
            if (criterion.Value is List<int> intList && intList.Count > 0)
            {
                List<string> paramNames = new();
                foreach (var value in intList)
                {
                    string paramName = $"@param{paramCounter++}";
                    paramNames.Add(paramName);
                    parameters.Add(new SqlParameter(paramName, value));
                }
                whereClauses.Add($"{criterion.Key} IN ({string.Join(", ", paramNames)})");
            }
            else if (criterion.Value is List<string> stringList && stringList.Count > 0)
            {
                List<string> paramNames = new();
                foreach (var value in stringList)
                {
                    string paramName = $"@param{paramCounter++}";
                    paramNames.Add(paramName);
                    parameters.Add(new SqlParameter(paramName, value));
                }
                whereClauses.Add($"{criterion.Key} IN ({string.Join(", ", paramNames)})");
            }
            else if (criterion.Value is string || criterion.Value is int || criterion.Value is bool || criterion.Value is DateTime || criterion.Value == null)
            {
                string paramName = $"@param{paramCounter++}";
                whereClauses.Add($"{criterion.Key} = {paramName}");
                parameters.Add(new SqlParameter(paramName, criterion.Value ?? DBNull.Value));
            }
            else
            {
                Console.WriteLine($"Unsupported parameter type: {criterion.Key} => {criterion.Value?.GetType().Name}");
            }
        }

        string finalQuery = queryString;
        if (whereClauses.Count > 0)
        {
            finalQuery += " WHERE " + string.Join(" AND ", whereClauses);
        }

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(finalQuery, connection);
        cmd.Parameters.AddRange(parameters.ToArray());

        try
        {
            await connection.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ITeam team = new Team
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
        catch (SqlException sqlEx)
        {
            Console.WriteLine("SQL Error (SearchTeamsAsync): " + sqlEx.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("General Error (SearchTeamsAsync): " + ex.Message);
        }

        return teams;
    }



    public async Task<bool> UpdateTeamAsync(ITeam team)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(updateString, connection);
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
        catch (SqlException sqlEx)
        {
            Console.WriteLine("SQL Error (UpdateTeamAsync): " + sqlEx.Message);
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("General Error (UpdateTeamAsync): " + ex.Message);
            return false;
        }
        finally
        {
            connection.Close();
        }
    }
}
