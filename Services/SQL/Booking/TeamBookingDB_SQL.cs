using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Gadevang_Tennis_Klub.Services.SQL.Booking
{
    public class TeamBookingDB_SQL : ITeamBookingDB
    {
        private string _connectionString = Secret.ConnectionString;
        private string insertString = "INSERT INTO TeamBookings (MemberID, TeamID) VALUES (@MemberID, @TeamID)";
        private string deleteString = "DELETE FROM TeamBookings WHERE ID = @ID";
        private string selectAllString = "SELECT ID, MemberID, TeamID FROM TeamBookings";
        private string selectByIdString = "SELECT ID, MemberID, TeamID FROM TeamBookings WHERE ID = @ID";
        private string updateString = "UPDATE TeamBookings SET MemberID = @MemberID, TeamID = @TeamID WHERE ID = @ID";

        public async Task<bool> CreateTeamBookingAsync(ITeamBooking teamBooking)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(insertString, connection);
            cmd.Parameters.AddWithValue("@MemberID", teamBooking.Member_ID);
            cmd.Parameters.AddWithValue("@TeamID", teamBooking.Team_ID);

            try
            {
                await connection.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Error (CreateTeamBookingAsync): " + sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error (CreateTeamBookingAsync): " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<bool> DeleteTeamBookingAsync(int teamBookingID)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(deleteString, connection);
            cmd.Parameters.AddWithValue("@ID", teamBookingID);

            try
            {
                await connection.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Error (DeleteTeamBookingAsync): " + sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error (DeleteTeamBookingAsync): " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<List<ITeamBooking>> GetAllTeamBookingAsync()
        {
            List<ITeamBooking> teamBookings = new();
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(selectAllString, connection);

            try
            {
                await connection.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    teamBookings.Add(new TeamBooking
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Member_ID = Convert.ToInt32(reader["MemberID"]),
                        Team_ID = Convert.ToInt32(reader["TeamID"])
                    });
                }

                reader.Close();
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Error (GetAllTeamBookingAsync): " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error (GetAllTeamBookingAsync): " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return teamBookings;
        }

        public async Task<ITeamBooking?> GetTeamBookingFromIDAsync(int teamBookingID)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(selectByIdString, connection);
            cmd.Parameters.AddWithValue("@ID", teamBookingID);

            try
            {
                await connection.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new TeamBooking
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Member_ID = Convert.ToInt32(reader["MemberID"]),
                        Team_ID = Convert.ToInt32(reader["TeamID"])
                    };
                }
                reader.Close();
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Error (GetTeamBookingFromIDAsync): " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error (GetTeamBookingFromIDAsync): " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public async Task<List<IMember>> GetMembersByTeamAsync(int teamId, IMemberDB memberDB)
        {
            var allBookings = await GetAllTeamBookingAsync();
            var bookingsForTeam = allBookings.Where(tb => tb.Team_ID == teamId).ToList();

            var members = new List<IMember>();

            foreach (var booking in bookingsForTeam)
            {
                var member = await memberDB.GetMemberByIDAsync(booking.Member_ID);
                if (member != null)
                {
                    members.Add(member);
                }
            }
            return members;
        }


        public async Task<bool> UpdateTeamBookingAsync(ITeamBooking teamBooking)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(updateString, connection);
            cmd.Parameters.AddWithValue("@ID", teamBooking.ID);
            cmd.Parameters.AddWithValue("@MemberID", teamBooking.Member_ID);
            cmd.Parameters.AddWithValue("@TeamID", teamBooking.Team_ID);

            try
            {
                await connection.OpenAsync();
                int affectedRows = await cmd.ExecuteNonQueryAsync();
                return affectedRows > 0;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Error (UpdateTeamBookingAsync): " + sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error (UpdateTeamBookingAsync): " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<int?> GetTeamBookingIDAsync(int teamID, int memberID)
        {
            string query = "SELECT ID FROM TeamBookings WHERE TeamID = @TeamID AND MemberID = @MemberID";

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@TeamID", teamID);
            cmd.Parameters.AddWithValue("@MemberID", memberID);

            try
            {
                await connection.OpenAsync();
                object result = await cmd.ExecuteScalarAsync();
                return result != null ? (int?)Convert.ToInt32(result) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetBookingIDAsync: " + ex.Message);
                return null;
            }
        }

    }
}
