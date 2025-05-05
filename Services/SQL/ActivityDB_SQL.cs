using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using Activity = Gadevang_Tennis_Klub.Models.Activity;

namespace Gadevang_Tennis_Klub.Services.SQL
{
    public class ActivityDB_SQL : IActivityDB
    {
        private readonly string _connectionString = Secret.ConnectionString;
        private readonly string _getAllActivitiesSql = "select ID, EventID, Description, Title, StartTime, Duration FROM Activities";
        private readonly string _getAllActivitiesByEventSql = "select ID, EventID, Description, Title, StartTime, Duration FROM Activities WHERE EventID = @EventID";
        private readonly string _insertSql = "insert INTO Activities Values(@EventID, @Description, @Title, @StartTime, @Duration)";
        private readonly string _deleteSql = "delete FROM Activities WHERE EventID = @EventID AND ID = @ID";
        private readonly string _updateEventSql = "update Activities SET Description = @Description, Title = @Title, StartTime = @StartTime, Duration = @Duration WHERE ID = @ID";
        private readonly string _getActivityByIDSql = "select ID, EventID, Description, Title, StartTime, Duration FROM Activities WHERE EventID = @EvID AND ID = @ID";

        public async Task<List<IActivity>> GetAllActivitiesAsync()
        {
            List<IActivity> activities = new List<IActivity>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_getAllActivitiesSql, connection);
                    command.Connection.Open();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32("ID");
                        int evID = reader.GetInt32("EventID");
                        string description = reader.GetString("Description");
                        string title = reader.GetString("Title");
                        DateTime startTime = reader.GetDateTime("StartTime");
                        TimeOnly endTime = reader.GetFieldValue<TimeOnly>("Duration");

                        IActivity activity = new Activity(id, evID, description, title, startTime, endTime);
                        activities.Add(activity);
                    }
                    reader.Close();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
            }
            return activities;
        }

        public async Task<bool> CreateActivityAsync(IActivity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_insertSql, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@EventID", activity.EventID);
                    command.Parameters.AddWithValue("@Description", activity.Description);
                    command.Parameters.AddWithValue("@Title", activity.Title);
                    command.Parameters.AddWithValue("@StartTime", activity.Start);
                    command.Parameters.AddWithValue("@Duration", activity.End);

                    int noOfRows = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Antal indsatte i tabellen {noOfRows}");

                    if (noOfRows == 1) return true;
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine(sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return false;
        }

        public async Task<bool> DeleteActivityAsync(int eventID, int activityID)
        {
            IActivity? activity = await GetActivityByIDAsync(eventID, activityID);
            if (activity == null) return false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_deleteSql, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@EventID", eventID);
                    command.Parameters.AddWithValue("@ID", activityID);

                    int noOfRows = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Antal fjernede i tabellen {noOfRows}");

                    if (noOfRows > 0) return true;
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine(sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return false;
        }

        public async Task<bool> UpdateActivityAsync(IActivity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_updateEventSql, connection);
                    command.Parameters.AddWithValue("@ID", activity.ID);

                    command.Parameters.AddWithValue("@Description", activity.Description);
                    command.Parameters.AddWithValue("@Title", activity.Title);
                    command.Parameters.AddWithValue("@StartTime", activity.Start);
                    command.Parameters.AddWithValue("@Duration", activity.End);

                    await command.Connection.OpenAsync();

                    int noOfRows = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Antal opdaterede i tabellen {noOfRows}");

                    if (noOfRows > 0) return true;
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
            }
            return false;
        }

        public async Task<IActivity?> GetActivityByIDAsync(int eventID, int activityID)
        {
            IActivity activity = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_getActivityByIDSql, connection);
                    command.Parameters.AddWithValue("@EvID", eventID);
                    command.Parameters.AddWithValue("@ID", activityID);

                    command.Connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32("ID");
                        int evID = reader.GetInt32("EventID");
                        string description = reader.GetString("Description");
                        string title = reader.GetString("Title");
                        DateTime startTime = reader.GetDateTime("StartTime");
                        TimeOnly endTime = reader.GetFieldValue<TimeOnly>("Duration");

                        activity = new Activity(id, evID, description, title, startTime, endTime);
                    }
                    reader.Close();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
            }
            return activity;
        }

        public async Task<List<IActivity>> GetActivitiesByEventAsync(int eventID)
        {
            List<IActivity> activities = new List<IActivity>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_getAllActivitiesByEventSql, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@EventID", eventID);

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32("ID");
                        int evID = reader.GetInt32("EventID");
                        string description = reader.GetString("Description");
                        string title = reader.GetString("Title");
                        DateTime startTime = reader.GetDateTime("StartTime");
                        TimeOnly endTime = reader.GetFieldValue<TimeOnly>("Duration");

                        IActivity activity = new Activity(id, evID, description, title, startTime, endTime);
                        activities.Add(activity);
                    }
                    reader.Close();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
            }
            return activities;
        }
    }
}
