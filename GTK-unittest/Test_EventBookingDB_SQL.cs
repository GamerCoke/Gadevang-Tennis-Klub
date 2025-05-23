using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Models;
using Gadevang_Tennis_Klub.Services.SQL;
using Gadevang_Tennis_Klub.Services.SQL.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTK_unittest.Kasper
{
    [TestClass]
    public class Test_EventBookingDB_SQL
    {
        [TestMethod]
        public void Test_GetAllEventBookingsAsync()
        {
            //Arrange
            ResetClass.Reset();
            EventBookingDB_SQL db = new EventBookingDB_SQL();

            //Act
            var list = db.GetAllEventBookingsAsync().Result;

            //Assert
            Assert.AreEqual(7, list.Count);

            Assert.AreEqual(0, list[0].ID);
            Assert.AreEqual(0, list[0].MemberID);
            Assert.AreEqual(0, list[0].EventID);

            Assert.AreEqual(1, list[1].ID);
            Assert.AreEqual(1, list[1].MemberID);
            Assert.AreEqual(2, list[1].EventID);

            Assert.AreEqual(2, list[2].ID);
            Assert.AreEqual(3, list[2].MemberID);
            Assert.AreEqual(2, list[2].EventID);

            Assert.AreEqual(3, list[3].ID);
            Assert.AreEqual(0, list[3].MemberID);
            Assert.AreEqual(4, list[3].EventID);

            Assert.AreEqual(4, list[4].ID);
            Assert.AreEqual(1, list[4].MemberID);
            Assert.AreEqual(6, list[4].EventID);

            Assert.AreEqual(5, list[5].ID);
            Assert.AreEqual(0, list[5].MemberID);
            Assert.AreEqual(6, list[5].EventID);

            Assert.AreEqual(6, list[6].ID);
            Assert.AreEqual(3, list[6].MemberID);
            Assert.AreEqual(6, list[6].EventID);

        }
        [TestMethod]
        public void Test_GetEventBookingByID()
        {
            //Arrange
            ResetClass.Reset();
            EventBookingDB_SQL db = new EventBookingDB_SQL();

            //Act
            var booking = db.GetEventBookingById(1).Result;

            //Assert
            Assert.AreEqual(1, booking.ID);
            Assert.AreEqual(1, booking.MemberID);
            Assert.AreEqual(2, booking.EventID);

        }
        [TestMethod]
        public void Test_CreateEventBookingAsync()
        {
            //Arrange
            ResetClass.Reset();
            EventBookingDB_SQL db = new EventBookingDB_SQL();

            //Act
            IEventBooking original = new EventBooking(1, 1);
            if (!db.CreateEventBookingAsync(original).Result)
                Assert.Fail();
            IEventBooking saved = db.GetEventBookingById(7).Result;

            //Assert
            Assert.AreEqual(7, saved.ID);
            Assert.AreEqual(1, saved.MemberID);
            Assert.AreEqual(1, saved.EventID);

        }
        [TestMethod]
        public void Test_DeleteEventBookingAsync()
        {
            //Arrange
            ResetClass.Reset();
            EventBookingDB_SQL db = new EventBookingDB_SQL();

            //Act
            if (!db.DeleteEventBookingAsync(0).Result)
                Assert.Fail();
            var list = db.GetAllEventBookingsAsync().Result;

            //Assert
            Assert.AreEqual(6, list.Count);

        }
        [TestMethod]
        public void Test_GetEventBookingsByMemberID()
        {
            //Arrange
            ResetClass.Reset();
            EventBookingDB_SQL db = new EventBookingDB_SQL();

            //Act
            var list = db.GetEventBookingsByMemberIDAsync(0).Result;

            //Assert
            Assert.AreEqual(3, list.Count);

            Assert.AreEqual(0, list[0].ID);
            Assert.AreEqual(0, list[0].MemberID);
            Assert.AreEqual(0, list[0].EventID);

            Assert.AreEqual(3, list[1].ID);
            Assert.AreEqual(0, list[1].MemberID);
            Assert.AreEqual(4, list[1].EventID);

            Assert.AreEqual(5, list[2].ID);
            Assert.AreEqual(0, list[2].MemberID);
            Assert.AreEqual(6, list[2].EventID);

        }
        [TestMethod]
        public void Test_GetEventBookingsByEventID()
        {
            //Arrange
            ResetClass.Reset();
            EventBookingDB_SQL db = new EventBookingDB_SQL();

            //Act
            var list = db.GetEventBookingsByEventIDAsync(6).Result;

            //Assert
            Assert.AreEqual(3, list.Count);

            Assert.AreEqual(4, list[0].ID);
            Assert.AreEqual(1, list[0].MemberID);
            Assert.AreEqual(6, list[0].EventID);

            Assert.AreEqual(5, list[1].ID);
            Assert.AreEqual(0, list[1].MemberID);
            Assert.AreEqual(6, list[1].EventID);

            Assert.AreEqual(6, list[2].ID);
            Assert.AreEqual(3, list[2].MemberID);
            Assert.AreEqual(6, list[2].EventID);

        }
    }
}
