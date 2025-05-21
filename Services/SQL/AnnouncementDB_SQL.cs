using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.Data.SqlClient;

namespace Gadevang_Tennis_Klub.Services.SQL
{
    public class AnnouncementDB_SQL : IAnnouncementDB
    {
        private string _connectionString = Secret.ConnectionString;
        string query = @"SELECT a.ID, a.Title, a.Text, a.Upload, a.Type, a.Actual,
                        m.ID AS MemberID, m.Name AS MemberName, m.Email AS MemberEmail
                 FROM Announcements a
                 INNER JOIN Members m ON a.MemberID = m.ID";

        public async Task<bool> CreateAnnouncementAsync(IAnnouncement announcement)
        {
            const string insertString = @"
INSERT INTO Announcements (MemberID, Title, Text, Upload, Type, Actual)
VALUES (@MemberID, @Title, @Text, @Upload, @Type, @Actual)";


            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(insertString, connection);

            // Use fallback ID 1 if Announcer is null or invalid
            int announcerId = (announcement.Announcer?.Id > 0) ? announcement.Announcer.Id : 1;

            cmd.Parameters.AddWithValue("@MemberID", announcerId);
            cmd.Parameters.AddWithValue("@Title", announcement.Title ?? "Untitled");
            cmd.Parameters.AddWithValue("@Text", announcement.Text ?? "");
            cmd.Parameters.AddWithValue("@Upload", announcement.UploadTime);
            cmd.Parameters.AddWithValue("@Type", announcement.Type ?? "General");
            cmd.Parameters.AddWithValue("@Actual", announcement.Type == "Service" ? (object?)announcement.Actual : DBNull.Value);


            try
            {
                await connection.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Error (CreateAnnouncementAsync): " + sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error (CreateAnnouncementAsync): " + ex.Message);
                return false;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }



        public async Task<List<IAnnouncement>> GetAllAnnouncementsAsync()
        {
            List<IAnnouncement> announcements = new();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                await connection.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    IMember announcer = new Member
                    {
                        Id = Convert.ToInt32(reader["MemberID"]),
                        Name = reader["MemberName"].ToString(),
                        Email = reader["MemberEmail"].ToString()
                    };

                    IAnnouncement announcement = new Announcement
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        Title = reader["Title"].ToString(),
                        Text = reader["Text"].ToString(),
                        UploadTime = Convert.ToDateTime(reader["Upload"]),
                        Type = reader["Type"].ToString(),
                        Announcer = announcer,
                        Actual = reader["Actual"] != DBNull.Value ? Convert.ToBoolean(reader["Actual"]) : null
                    };


                    announcements.Add(announcement);
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Error (GetAllAnnouncementsAsync): " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error (GetAllAnnouncementsAsync): " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return announcements;
        }

        public async Task<List<IAnnouncement>> SearchAnnouncementsAsync(Dictionary<string, (string Operator, object Value)> searchCriteria)
        {
            List<IAnnouncement> announcements = new();
            string baseQuery = @"
SELECT A.ID, A.Title, A.Text, A.Upload, A.Type, A.Actual, M.ID AS MemberID, M.Name, M.Email, M.Phone
FROM Announcements A
JOIN Members M ON A.MemberID = M.ID";

            List<string> whereClauses = new();
            List<SqlParameter> parameters = new();
            int paramCounter = 0;

            foreach (var criterion in searchCriteria)
            {
                string column = criterion.Key;
                string op = criterion.Value.Operator;
                object value = criterion.Value.Value;

                // Remap special keys to actual SQL column names
                if (column == "UploadStart" || column == "UploadEnd")
                    column = "Upload";

                if (value is int || value is string || value is DateTime)
                {
                    string paramName = $"@param{paramCounter++}";
                    whereClauses.Add($"A.{column} {op} {paramName}");
                    parameters.Add(new SqlParameter(paramName, value));
                }
                else
                {
                    Console.WriteLine($"Unsupported parameter type for {column}");
                }
            }


            string finalQuery = baseQuery;
            if (whereClauses.Count > 0)
                finalQuery += " WHERE " + string.Join(" AND ", whereClauses);

            using SqlConnection connection = new SqlConnection(Secret.ConnectionString);
            using SqlCommand cmd = new SqlCommand(finalQuery, connection);
            cmd.Parameters.AddRange(parameters.ToArray());

            try
            {
                await connection.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    IAnnouncement announcement = new Announcement(
                    id: Convert.ToInt32(reader["ID"]),
                    title: reader["Title"].ToString(),
                    text: reader["Text"].ToString(),
                    uploadTime: Convert.ToDateTime(reader["Upload"]),
                    type: reader["Type"].ToString(),
                    announcer: new Member
                    {
                        Id = Convert.ToInt32(reader["MemberID"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString()
                    }
                    );

                    announcement.Actual = reader["Actual"] != DBNull.Value ? Convert.ToBoolean(reader["Actual"]) : null;


                    announcements.Add(announcement);
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Error (SearchAnnouncementsAsync): " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error (SearchAnnouncementsAsync): " + ex.Message);
            }

            return announcements;
        }

        public async Task<IAnnouncement?> GetAnnouncementByIDAsync(int id)
        {
            var criteria = new Dictionary<string, (string, object)>
            {
                { "ID", ("=", id) }
            };
            return (await SearchAnnouncementsAsync(criteria)).FirstOrDefault();
        }


        public async Task<List<IAnnouncement>> GetAnnouncementsByTypeAsync(string type)
        {
            var criteria = new Dictionary<string, (string, object)>
            {
                { "Type", ("=", type) }
            };
            return await SearchAnnouncementsAsync(criteria);
        }



        public async Task<List<IAnnouncement>> GetAnnouncementsByDateIntervalAsync(DateTime date1, DateTime date2)
        {
            var criteria = new Dictionary<string, (string, object)>
            {
                { "UploadStart", (">=", date1) },
                { "UploadEnd", ("<=", date2) }
            };

            return await SearchAnnouncementsAsync(criteria);
        }



        public async Task<List<IAnnouncement>> GetAnnouncementsByDateOlderThanAsync(DateTime date)
        {
            var criteria = new Dictionary<string, (string, object)>
            {
                { "Upload", ("<", date) }
            };
            return await SearchAnnouncementsAsync(criteria);
        }


        public async Task<List<IAnnouncement>> GetAnnouncementsAfterDateAsync(DateTime date)
        {
            var criteria = new Dictionary<string, (string, object)>
            {
                { "Upload", (">", date) }
            };
            return await SearchAnnouncementsAsync(criteria);
        }


        public async Task<List<IAnnouncement>> UpdateAnnouncementAsync(IAnnouncement announcement)
        {
            string query = @"
UPDATE Announcements
SET Title = @Title, Text = @Text, Upload = @Upload, Type = @Type, Actual = @Actual
WHERE ID = @ID";


            using SqlConnection connection = new SqlConnection(Secret.ConnectionString);
            using SqlCommand cmd = new SqlCommand(query, connection);

            // Set the parameters for the fields we are updating
            cmd.Parameters.AddWithValue("@Title", announcement.Title);
            cmd.Parameters.AddWithValue("@Text", announcement.Text);
            cmd.Parameters.AddWithValue("@Upload", announcement.UploadTime);
            cmd.Parameters.AddWithValue("@Type", announcement.Type);
            cmd.Parameters.AddWithValue("@ID", announcement.Id);
            cmd.Parameters.AddWithValue("@Actual", announcement.Type == "Service" ? (object?)announcement.Actual : DBNull.Value);

            try
            {
                await connection.OpenAsync();

                // Execute the update command
                int affectedRows = await cmd.ExecuteNonQueryAsync();

                if (affectedRows > 0)
                {
                    // After updating, return the updated announcement for confirmation
                    return await SearchAnnouncementsAsync(new Dictionary<string, (string, object)>
            {
                { "ID", ("=", announcement.Id) }
            });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error (UpdateAnnouncementAsync): " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error (UpdateAnnouncementAsync): " + ex.Message);
            }

            return new List<IAnnouncement>(); // Return an empty list if no update happened
        }


        public async Task<bool> DeleteAnnouncementAsync(int announcementID)
        {
            string query = "DELETE FROM Announcements WHERE ID = @ID";

            using SqlConnection connection = new SqlConnection(Secret.ConnectionString);
            using SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ID", announcementID);

            try
            {
                await connection.OpenAsync();
                int affectedRows = await cmd.ExecuteNonQueryAsync();
                return affectedRows > 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error (DeleteAnnouncementAsync): " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error (DeleteAnnouncementAsync): " + ex.Message);
            }

            return false;
        }

    }
}
