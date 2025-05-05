using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace Gadevang_Tennis_Klub.Services.SQL.Booking
{
    public class CourtBookingDB_SQL : ICourtBookingDB
    {
        private string connectionString = Secret.ConnectionString;
        public async Task<bool> CreateCourtBookingAsync(ICourtBooking courtBooking)
        {
            string teamId = courtBooking.Team_ID == null ? "null" : courtBooking.Team_ID.ToString();
            string memberId = courtBooking.Member_ID == null ? "null" : courtBooking.Member_ID.ToString();
            string eventId = courtBooking.Event_ID == null ? "null" : courtBooking.Event_ID.ToString();
            string query = "insert into CourtBookings values (" +
                $"'{courtBooking.Date.ToString()}', " +//Date
                $"{courtBooking.Court_ID}, " +//Court
                $"{courtBooking.Timeslot}, " +//Timeslot
                $"{teamId}, " +//Team
                $"{memberId}, " +//Member
                $"{eventId}" +//Event
                ")";
            return await NonReadQueryAsync(query);
        }

        public async Task<bool> DeleteCourtBookingAsync(int courtBookingID)
        {
            return await NonReadQueryAsync($"delete from CourtBookings where Id = {courtBookingID};");
        }

        public async Task<List<ICourtBooking>> GetAllCourtBookingsAsync()
        {
            return await GetBookingsViaQueryAsync(@"select * from CourtBookings;");
        }

        public async Task<ICourtBooking> GetCourtBookingByIDAsync(int courtBookingID)
        {
            return (await GetBookingsViaQueryAsync($"select * from CourtBookings where ID = {courtBookingID};")).First();
        }

        public async Task<List<ICourtBooking>> GetCourtBookingsByCourtIDAsync(int courtID)
        {
            return await GetBookingsViaQueryAsync($"select * from CourtBookings where CourtID = {courtID};");
        }

        public async Task<List<ICourtBooking>> GetCourtBookingsByEventIDAsync(int eventID)
        {
            return await GetBookingsViaQueryAsync($"select * from CourtBookings where EventID = {eventID};");
        }

        public async Task<List<ICourtBooking>> GetCourtBookingsByOrganiserAsync(int memberID)
        {
            return await GetBookingsViaQueryAsync($"select * from CourtBookings where MemberID = {memberID};");
        }

        public async Task<List<ICourtBooking>> GetCourtBookingsByParticipantsAsync(int memberID)
        {
            return await GetBookingsViaQueryAsync($"select CourtBookings.* from Partners join CourtBookings on CourtBookings.ID = Partners.BookingID where Partners.MemberID = {memberID} or CourtBookings.MemberID = {memberID};");
        }

        public async Task<List<ICourtBooking>> GetCourtBookingsByTeamIDAsync(int teamID)
        {
            return await GetBookingsViaQueryAsync($"select * from CourtBookings where TeamID = {teamID};");
        }

        public async Task<bool> UpdateCourtBookingAsync(ICourtBooking courtBooking)
        {
            DateOnly dateOnly = courtBooking.Date;
            string dateString = $"{dateOnly.Day}-{dateOnly.Month}-{dateOnly.Year}";
            string query = $"update CourtBookings " +
                $"set " +
                $"BookingDate = '{dateString}', " +
                $"CourtID = {courtBooking.Court_ID}, " +
                $"TimeSlot = {courtBooking.Timeslot} " +
                $"where ID = {courtBooking.ID};";
            return await NonReadQueryAsync(query);
        }

        public async Task<bool> AddPartisipantAsync(int bookingID, int memberID)
        {
            ICourtBooking booking = await GetCourtBookingByIDAsync(bookingID);
            if (booking.Participants == null)
                return false;
            else if (booking.Member_ID == memberID)
                return false;
            else if (booking.Participants.Count >= 3)
                return false;

            return await NonReadQueryAsync($"insert into Partners values({memberID}, {bookingID}); ");
        }

        public async Task<bool> RemovePartisipantAsync(int bookingID, int memberID)
        {
            return await NonReadQueryAsync($"delete from Partners where MemberID = {memberID} and BookingID = {bookingID};");
        }

        private async Task<List<ICourtBooking>> GetBookingsViaQueryAsync(string query)
        {
            List<ICourtBooking> bookings = new();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    bookings.Add(await GetBookingFromReaderAsync(reader));
                }
                reader.Close();

                foreach (ICourtBooking booking in bookings)
                    if (booking.Member_ID != null)
                        await AddParticipantsAsync(booking);
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
        private async Task<ICourtBooking> GetBookingFromReaderAsync(SqlDataReader reader)
        {
            int ID = reader.GetInt32(0);
            DateOnly date = DateOnly.FromDateTime(reader.GetDateTime(1));
            int courtID = reader.GetInt32(2);
            int timeslot = reader.GetInt32(3);

            int? teamId;
            if (!await reader.IsDBNullAsync(4))
                teamId = reader.GetInt32(4);
            else
                teamId = null;

            int? memberId;
            if (!await reader.IsDBNullAsync(5))
                memberId = reader.GetInt32(5);
            else
                memberId = null;

            int? eventID;
            if (!await reader.IsDBNullAsync(6))
                eventID = reader.GetInt32(6);
            else
                eventID = null;

            ICourtBooking booking =
                new CourtBooking(ID, courtID, date, timeslot, teamId, memberId, eventID);

            return booking;
        }
        private async Task AddParticipantsAsync(ICourtBooking booking)
        {
            string queryString = $"select Members.* from Partners join Members on Members.ID = Partners.MemberID where BookingID = {booking.ID};";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    List<IMember> participants = new();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string password = reader.GetString(2);
                        string address = reader.GetString(3);
                        string email = reader.GetString(4);
                        string phone = reader.GetString(5);
                        string sex = reader.GetString(6);
                        DateOnly dob = DateOnly.FromDateTime(reader.GetDateTime(7));
                        string bio = reader.GetString(8);
                        bool isAdmin = reader.GetBoolean(9);

                        string? image;
                        if (!await reader.IsDBNullAsync(10))
                            image = reader.GetString(10);
                        else
                            image = null;

                        IMember member =
                            new Member(id, name, phone, email, image, sex, isAdmin, bio, address, password);
                        participants.Add(member);
                    }
                    booking.Participants = participants;
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine($"Database error: " + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Generel fejl ved: " + ex.Message);
                }
                finally
                {

                }
            }
        }

        public async Task<bool> RemovePartisipant(int memberID)
        {
            throw new NotImplementedException();

        }
    }
}