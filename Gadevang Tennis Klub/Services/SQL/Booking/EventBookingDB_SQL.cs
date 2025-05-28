using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Gadevang_Tennis_Klub.Services.SQL.Booking
{
    public class EventBookingDB_SQL : IEventBookingDB
    {
        private string connectionString = Secret.ConnectionString;
        public async Task<bool> CreateEventBookingAsync(IEventBooking eventBooking)
        {
            return await NonReadQueryAsync($"insert into EventBookings values ({eventBooking.MemberID}, {eventBooking.EventID});");
        }

        public async Task<bool> DeleteEventBookingAsync(int eventBookingID)
        {
            return await NonReadQueryAsync($"delete from EventBookings where ID = {eventBookingID};");
        }

        public async Task<List<IEventBooking>> GetAllEventBookingsAsync()
        {
            return await GetBookingsViaQueryAsync($"select * from EventBookings;");
        }

        public async Task<IEventBooking> GetEventBookingById(int eventBookingID)
        {
            return (await GetBookingsViaQueryAsync($"select * from EventBookings where ID = {eventBookingID};")).First();
        }
        public async Task<List<IEventBooking>> GetEventBookingsByMemberIDAsync(int memberID)
        {
            return await GetBookingsViaQueryAsync($"select * from EventBookings where MemberID = {memberID};");
        }

        public async Task<List<IEventBooking>> GetEventBookingsByEventIDAsync(int eventID)
        {
            return await GetBookingsViaQueryAsync($"select * from EventBookings where EventID = {eventID};");
        }
        private async Task<List<IEventBooking>> GetBookingsViaQueryAsync(string query)
        {
            List<IEventBooking> bookings = new();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    bookings.Add(GetBookingFromReaderAsync(reader));
                }
                reader.Close();
            }
            return bookings;
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
        private IEventBooking GetBookingFromReaderAsync(SqlDataReader reader)
        {
            int ID = reader.GetInt32(0);
            int memberID = reader.GetInt32(1);
            int eventID = reader.GetInt32(2);

            IEventBooking booking = new EventBooking(ID, memberID, eventID);
            return booking;
        }

    }
}
