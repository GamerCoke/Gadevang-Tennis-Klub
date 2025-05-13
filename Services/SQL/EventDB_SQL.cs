using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Gadevang_Tennis_Klub.Services.SQL
{
    public class EventDB_SQL : IEventDB
    {
        private readonly string _connectionString = Secret.ConnectionString;
        private readonly string _getAllEventsSql = "select ID, Title, Description, StartTime, EndTime, Location, Capacity FROM Events";
        private readonly string _insertSql = "insert into Events (Title, Description, StartTime, EndTime, Location, Capacity) OUTPUT INSERTED.ID VALUES (@Title, @Description, @StartTime, @EndTime, @Location, @Capacity);";
        private readonly string _deleteSql = "delete FROM Events WHERE ID = @ID";
        private readonly string _updateEventSql = "update Events SET Title = @Title, Description = @Description, StartTime = @StartTime, EndTime = @EndTime, Location = @Location, Capacity = @Capacity WHERE ID = @ID";
        private readonly string _getEventByIDSql = "select ID, Title, Description, StartTime, EndTime, Location, Capacity FROM Events WHERE ID = @ID";
        private readonly string _getEventsByDateSql = "select * from Events WHERE CAST(StartTime AS DATE) = @Start";

        public async Task<List<IEvent>> GetAllEventsAsync()
        {
            List<IEvent> events = new List<IEvent>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_getAllEventsSql, connection);
                    command.Connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32("ID");
                        string title = reader.GetString("Title");
                        string desc = reader.GetString("Description");
                        DateTime startTime = reader.GetDateTime("StartTime");
                        TimeOnly endTime = reader.GetFieldValue<TimeOnly>("EndTime");
                        string location = reader.GetString("Location");
                        int? capacity = reader.IsDBNull("Capacity") ? null : reader.GetInt32("Capacity");

                        Event ev = new Event(id, title, desc, startTime, endTime, location, capacity);
                        events.Add(ev);
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
            return events;
        }

        public async Task<int?> CreateEventAsync(IEvent ev)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_insertSql, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@Title", ev.Title);
                    command.Parameters.AddWithValue("@Description", ev.Description);
                    command.Parameters.AddWithValue("@StartTime", ev.Start);
                    command.Parameters.AddWithValue("@EndTime", ev.End);
                    command.Parameters.AddWithValue("@Location", ev.Location);
                    command.Parameters.AddWithValue("@Capacity", ev.Capacity ?? (object)DBNull.Value);

                    object result = await command.ExecuteScalarAsync(); // Tries to retrieve the event id it got from the database.
                    return result != null ? Convert.ToInt32(result) : null;
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
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            IEvent? ev = await GetEventByIDAsync(eventId);
            if (ev == null) return false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_deleteSql, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@ID", eventId);

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

        public async Task<bool> UpdateEventAsync(IEvent ev)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_updateEventSql, connection);
                    command.Parameters.AddWithValue("@ID", ev.ID);
                    command.Parameters.AddWithValue("@Title", ev.Title);
                    command.Parameters.AddWithValue("@Description", ev.Description);
                    command.Parameters.AddWithValue("@StartTime", ev.Start);
                    command.Parameters.AddWithValue("@EndTime", ev.End);
                    command.Parameters.AddWithValue("@Location", ev.Location);
                    command.Parameters.AddWithValue("@Capacity", ev.Capacity ?? (object)DBNull.Value);

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

        public async Task<IEvent?> GetEventByIDAsync(int eventId)
        {
            IEvent ev = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_getEventByIDSql, connection);
                    command.Parameters.AddWithValue("@ID", eventId);

                    command.Connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32("ID");
                        string title = reader.GetString("Title");
                        string desc = reader.GetString("Description");
                        DateTime startTime = reader.GetDateTime("StartTime");
                        TimeOnly endTime = reader.GetFieldValue<TimeOnly>("EndTime");
                        string location = reader.GetString("Location");
                        int? capacity = reader.IsDBNull("Capacity") ? null : reader.GetInt32("Capacity");

                        ev = new Event(id, title, desc, startTime, endTime, location, capacity);
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
            return ev;
        }

        public async Task<int?> GetEventCapacityAsync(int eventID)
        {
            IEvent? ev = await GetEventByIDAsync(eventID);
            if (ev == null)
            {
                throw new Exception($"Et event med eventID: {eventID} kunne ikke findes");
            }
            return ev.Capacity;
        }

        public async Task<List<IEvent>> GetEventsByDateAsync(DateTime date)
        {
            List<IEvent> events = new List<IEvent>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_getEventsByDateSql, connection);
                    command.Parameters.AddWithValue("@Start", date.Date);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("ID");
                        string title = reader.GetString("Title");
                        string desc = reader.GetString("Description");
                        DateTime startTime = reader.GetDateTime("StartTime");
                        TimeOnly endTime = reader.GetFieldValue<TimeOnly>("EndTime");
                        string location = reader.GetString("Location");
                        int? capacity = reader.IsDBNull("Capacity") ? null : reader.GetInt32("Capacity");

                        Event ev = new Event(id, title, desc, startTime, endTime, location, capacity);
                        events.Add(ev);
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
            return events;
        }

        public List<IEvent> SortEventsByDate(List<IEvent> listToSort)
        {
            List<IEvent> sortedList = listToSort;
            sortedList.Sort((d1, d2) => d1.Start.CompareTo(d2.Start));

            return sortedList;
        }
    }
}
