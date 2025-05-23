using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Models;
using Gadevang_Tennis_Klub.Services.SQL.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTK_unittest.Kasper
{
    [TestClass]
    public class Test_CourtBookingDB_SQL
    {
        [TestMethod]
        public void Test_CreateCourtBookingAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            ICourtBooking original = new CourtBooking(0, 2, new DateOnly(2001, 1, 1), 0, null, 3, null);
            if (!db.CreateCourtBookingAsync(original).Result)
                Assert.Fail();
            ICourtBooking saved = db.GetCourtBookingByIDAsync(9).Result;

            //Assert
            Assert.AreEqual(original.Date, saved.Date);
            Assert.AreEqual(original.Court_ID, saved.Court_ID);
            Assert.AreEqual(original.Timeslot, saved.Timeslot);
            Assert.AreEqual(original.Team_ID, saved.Team_ID);
            Assert.AreEqual(original.Member_ID, saved.Member_ID);
            Assert.AreEqual(original.Event_ID, saved.Event_ID);

        }
        private class TestIvalidCourtBooking : ICourtBooking
        {
            public DateOnly Date { get; set; } = new DateOnly(2001, 1, 1);
            public int Court_ID { get; set; } = 2;
            public int Timeslot { get; set; } = 0;
            public int ID { get; set; } = 0;
            public IReadOnlyList<IMember>? Participants { get; set; } = null;
            public int? Team_ID { get; set; } = null;
            public int? Member_ID { get; set; } = 3;
            public int? Event_ID { get; set; } = 1;
        }
        [TestMethod]
        public void Test_InvalidCreateCourtBookingAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            ICourtBooking original = new TestIvalidCourtBooking();
            bool isCreated = true;
            try
            {
                isCreated = db.CreateCourtBookingAsync(original).Result;
            }
            catch (Exception ex)
            {
                isCreated = false;
            }

            //Assert
            if (isCreated)
                Assert.Fail();

        }
        [TestMethod]
        public void Test_UpdateCourtBookingAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            ICourtBooking original = new CourtBooking(0, 2, new DateOnly(2001, 1, 1), 0, null, 1, null);
            //MemberID, EventID, TeamID may not be changed
            if (!db.UpdateCourtBookingAsync(original).Result)
                Assert.Fail();
            ICourtBooking updated = db.GetCourtBookingByIDAsync(0).Result;

            //Assert
            Assert.AreEqual(original.Date, updated.Date);
            Assert.AreEqual(original.Court_ID, updated.Court_ID);
            Assert.AreEqual(original.Timeslot, updated.Timeslot);
            Assert.AreEqual(original.Team_ID, updated.Team_ID);
            Assert.AreEqual(original.Member_ID, updated.Member_ID);
            Assert.AreEqual(original.Event_ID, updated.Event_ID);
        }
        [TestMethod]
        public void Test_DeleteCourtBookingAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            if (!db.DeleteCourtBookingAsync(2).Result)
                Assert.Fail();
            int count = db.GetAllCourtBookingsAsync().Result.Count;

            //Assert
            Assert.AreEqual(8, count);

        }
        [TestMethod]
        public void Test_GetAllCourtBookingsAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            var list = db.GetAllCourtBookingsAsync().Result;

            //Assert
            Assert.AreEqual(9, list.Count);

            Assert.AreEqual(2, list[0].ID);
            Assert.AreEqual(new DateOnly(2025, 06, 11), list[0].Date);
            Assert.AreEqual(0, list[0].Court_ID);
            Assert.AreEqual(4, list[0].Timeslot);
            Assert.AreEqual(null, list[0].Team_ID);
            Assert.AreEqual(2, list[0].Member_ID);
            Assert.AreEqual(null, list[0].Event_ID);

            Assert.AreEqual(3, list[1].ID);
            Assert.AreEqual(new DateOnly(2025, 05, 21), list[1].Date);
            Assert.AreEqual(0, list[1].Court_ID);
            Assert.AreEqual(9, list[1].Timeslot);
            Assert.AreEqual(1, list[1].Team_ID);
            Assert.AreEqual(null, list[1].Member_ID);
            Assert.AreEqual(null, list[1].Event_ID);

            Assert.AreEqual(7, list[2].ID);
            Assert.AreEqual(new DateOnly(2025, 05, 25), list[2].Date);
            Assert.AreEqual(0, list[2].Court_ID);
            Assert.AreEqual(11, list[2].Timeslot);
            Assert.AreEqual(null, list[2].Team_ID);
            Assert.AreEqual(null, list[2].Member_ID);
            Assert.AreEqual(1, list[2].Event_ID);

            Assert.AreEqual(0, list[3].ID);
            Assert.AreEqual(new DateOnly(2025, 06, 15), list[3].Date);
            Assert.AreEqual(1, list[3].Court_ID);
            Assert.AreEqual(1, list[3].Timeslot);
            Assert.AreEqual(null, list[3].Team_ID);
            Assert.AreEqual(1, list[3].Member_ID);
            Assert.AreEqual(null, list[3].Event_ID);

            Assert.AreEqual(1, list[4].ID);
            Assert.AreEqual(new DateOnly(2025, 07, 22), list[4].Date);
            Assert.AreEqual(1, list[4].Court_ID);
            Assert.AreEqual(3, list[4].Timeslot);
            Assert.AreEqual(null, list[4].Team_ID);
            Assert.AreEqual(0, list[4].Member_ID);
            Assert.AreEqual(null, list[4].Event_ID);

            Assert.AreEqual(8, list[5].ID);
            Assert.AreEqual(new DateOnly(2025, 08, 19), list[5].Date);
            Assert.AreEqual(1, list[5].Court_ID);
            Assert.AreEqual(4, list[5].Timeslot);
            Assert.AreEqual(null, list[5].Team_ID);
            Assert.AreEqual(null, list[5].Member_ID);
            Assert.AreEqual(2, list[5].Event_ID);

            Assert.AreEqual(5, list[6].ID);
            Assert.AreEqual(new DateOnly(2025, 06, 13), list[6].Date);
            Assert.AreEqual(1, list[6].Court_ID);
            Assert.AreEqual(8, list[6].Timeslot);
            Assert.AreEqual(0, list[6].Team_ID);
            Assert.AreEqual(null, list[6].Member_ID);
            Assert.AreEqual(null, list[6].Event_ID);

            Assert.AreEqual(4, list[7].ID);
            Assert.AreEqual(new DateOnly(2025, 08, 06), list[7].Date);
            Assert.AreEqual(2, list[7].Court_ID);
            Assert.AreEqual(1, list[7].Timeslot);
            Assert.AreEqual(2, list[7].Team_ID);
            Assert.AreEqual(null, list[7].Member_ID);
            Assert.AreEqual(null, list[7].Event_ID);

            Assert.AreEqual(6, list[8].ID);
            Assert.AreEqual(new DateOnly(2025, 07, 08), list[8].Date);
            Assert.AreEqual(2, list[8].Court_ID);
            Assert.AreEqual(3, list[8].Timeslot);
            Assert.AreEqual(null, list[8].Team_ID);
            Assert.AreEqual(null, list[8].Member_ID);
            Assert.AreEqual(0, list[8].Event_ID);

        }
        [TestMethod]
        public void Test_GetCourtBookingByIDAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            var booking = db.GetCourtBookingByIDAsync(2).Result;

            //Assert
            Assert.AreEqual(2, booking.ID);
            Assert.AreEqual(new DateOnly(2025, 06, 11), booking.Date);
            Assert.AreEqual(0, booking.Court_ID);
            Assert.AreEqual(4, booking.Timeslot);
            Assert.AreEqual(null, booking.Team_ID);
            Assert.AreEqual(2, booking.Member_ID);
            Assert.AreEqual(null, booking.Event_ID);
            Assert.AreEqual(1, booking.Participants.Count);
            Assert.AreEqual(0, booking.Participants[0].Id);

        }
        [TestMethod]
        public void Test_GetCourtBookingsByCourtIDAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            var list = db.GetCourtBookingsByCourtIDAsync(1).Result;

            //Assert
            Assert.AreEqual(4, list.Count);

            Assert.AreEqual(0, list[0].ID);
            Assert.AreEqual(new DateOnly(2025, 06, 15), list[0].Date);
            Assert.AreEqual(1, list[0].Court_ID);
            Assert.AreEqual(1, list[0].Timeslot);
            Assert.AreEqual(null, list[0].Team_ID);
            Assert.AreEqual(1, list[0].Member_ID);
            Assert.AreEqual(null, list[0].Event_ID);

            Assert.AreEqual(1, list[1].ID);
            Assert.AreEqual(new DateOnly(2025, 07, 22), list[1].Date);
            Assert.AreEqual(1, list[1].Court_ID);
            Assert.AreEqual(3, list[1].Timeslot);
            Assert.AreEqual(null, list[1].Team_ID);
            Assert.AreEqual(0, list[1].Member_ID);
            Assert.AreEqual(null, list[1].Event_ID);

            Assert.AreEqual(8, list[2].ID);
            Assert.AreEqual(new DateOnly(2025, 08, 19), list[2].Date);
            Assert.AreEqual(1, list[2].Court_ID);
            Assert.AreEqual(4, list[2].Timeslot);
            Assert.AreEqual(null, list[2].Team_ID);
            Assert.AreEqual(null, list[2].Member_ID);
            Assert.AreEqual(2, list[2].Event_ID);

            Assert.AreEqual(5, list[3].ID);
            Assert.AreEqual(new DateOnly(2025, 06, 13), list[3].Date);
            Assert.AreEqual(1, list[3].Court_ID);
            Assert.AreEqual(8, list[3].Timeslot);
            Assert.AreEqual(0, list[3].Team_ID);
            Assert.AreEqual(null, list[3].Member_ID);
            Assert.AreEqual(null, list[3].Event_ID);

        }
        [TestMethod]
        public void Test_GetCourtBookingsByEventIDAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            var list = db.GetCourtBookingsByEventIDAsync(1).Result;

            //Assert
            Assert.AreEqual(1, list.Count);

            Assert.AreEqual(7, list[0].ID);
            Assert.AreEqual(new DateOnly(2025, 05, 25), list[0].Date);
            Assert.AreEqual(0, list[0].Court_ID);
            Assert.AreEqual(11, list[0].Timeslot);
            Assert.AreEqual(null, list[0].Team_ID);
            Assert.AreEqual(null, list[0].Member_ID);
            Assert.AreEqual(1, list[0].Event_ID);

        }
        [TestMethod]
        public void Test_GetCourtBookingsByTeamIDAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            var list = db.GetCourtBookingsByTeamIDAsync(1).Result;

            //Assert
            Assert.AreEqual(1, list.Count);

            Assert.AreEqual(3, list[0].ID);
            Assert.AreEqual(new DateOnly(2025, 05, 21), list[0].Date);
            Assert.AreEqual(0, list[0].Court_ID);
            Assert.AreEqual(9, list[0].Timeslot);
            Assert.AreEqual(1, list[0].Team_ID);
            Assert.AreEqual(null, list[0].Member_ID);
            Assert.AreEqual(null, list[0].Event_ID);

        }
        [TestMethod]
        public void Test_GetCourtBookingsByOrganiserAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            var list = db.GetCourtBookingsByOrganiserAsync(2).Result;

            //Assert
            Assert.AreEqual(1, list.Count);

            Assert.AreEqual(2, list[0].ID);
            Assert.AreEqual(new DateOnly(2025, 06, 11), list[0].Date);
            Assert.AreEqual(0, list[0].Court_ID);
            Assert.AreEqual(4, list[0].Timeslot);
            Assert.AreEqual(null, list[0].Team_ID);
            Assert.AreEqual(2, list[0].Member_ID);
            Assert.AreEqual(null, list[0].Event_ID);

        }
        [TestMethod]
        public void Test_GetCourtBookingsByParticipantsAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            var list = db.GetCourtBookingsByParticipantsAsync(0).Result;

            //Assert
            Assert.AreEqual(3, list.Count);

            Assert.AreEqual(2, list[0].ID);
            Assert.AreEqual(new DateOnly(2025, 06, 11), list[0].Date);
            Assert.AreEqual(0, list[0].Court_ID);
            Assert.AreEqual(4, list[0].Timeslot);
            Assert.AreEqual(null, list[0].Team_ID);
            Assert.AreEqual(2, list[0].Member_ID);
            Assert.AreEqual(null, list[0].Event_ID);

            Assert.AreEqual(0, list[1].ID);
            Assert.AreEqual(new DateOnly(2025, 06, 15), list[1].Date);
            Assert.AreEqual(1, list[1].Court_ID);
            Assert.AreEqual(1, list[1].Timeslot);
            Assert.AreEqual(null, list[1].Team_ID);
            Assert.AreEqual(1, list[1].Member_ID);
            Assert.AreEqual(null, list[1].Event_ID);

            Assert.AreEqual(1, list[2].ID);
            Assert.AreEqual(new DateOnly(2025, 07, 22), list[2].Date);
            Assert.AreEqual(1, list[2].Court_ID);
            Assert.AreEqual(3, list[2].Timeslot);
            Assert.AreEqual(null, list[2].Team_ID);
            Assert.AreEqual(0, list[2].Member_ID);
            Assert.AreEqual(null, list[2].Event_ID);

        }
        [TestMethod]
        public void Test_AddPartisipantAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            if (!db.AddPartisipantAsync(2, 3).Result)
                Assert.Fail();
            var booking = db.GetCourtBookingByIDAsync(2).Result;

            //Assert
            Assert.AreEqual(2, booking.ID);
            Assert.AreEqual(new DateOnly(2025, 06, 11), booking.Date);
            Assert.AreEqual(0, booking.Court_ID);
            Assert.AreEqual(4, booking.Timeslot);
            Assert.AreEqual(null, booking.Team_ID);
            Assert.AreEqual(2, booking.Member_ID);
            Assert.AreEqual(null, booking.Event_ID);
            Assert.AreEqual(2, booking.Participants.Count);
            Assert.AreEqual(0, booking.Participants[0].Id);
            Assert.AreEqual(3, booking.Participants[1].Id);
        }
        [TestMethod]
        public void Test_RemovePartisipantAsync()
        {
            //Arrange
            ResetClass.Reset();
            CourtBookingDB_SQL db = new CourtBookingDB_SQL();

            //Act
            if (!db.RemovePartisipantAsync(2, 0).Result)
                Assert.Fail();
            var booking = db.GetCourtBookingByIDAsync(2).Result;

            //Assert
            Assert.AreEqual(2, booking.ID);
            Assert.AreEqual(new DateOnly(2025, 06, 11), booking.Date);
            Assert.AreEqual(0, booking.Court_ID);
            Assert.AreEqual(4, booking.Timeslot);
            Assert.AreEqual(null, booking.Team_ID);
            Assert.AreEqual(2, booking.Member_ID);
            Assert.AreEqual(null, booking.Event_ID);
            Assert.AreEqual(0, booking.Participants.Count);
        }
    }
}
