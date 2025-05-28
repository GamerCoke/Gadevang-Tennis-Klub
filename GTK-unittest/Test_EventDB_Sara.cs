using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Gadevang_Tennis_Klub.Services.SQL;
using Microsoft.Data.SqlClient;

namespace GTK_unittest
{
    [TestClass]
    public sealed class Test_EventDB_Sara
    {
        #region EventDB Tests
        [TestMethod]
        public async Task TestCreateEventAsyncSuccessfull()
        {
            // Arrange
            IEventDB eventDB = new EventDB_SQL();
            IEvent newEvent = new Event("TestTitle", "TestDescription", DateTime.Today, TimeOnly.Parse("11:00:00"), "TestRoad 123", 10);

            // Act
            int? eventID = await eventDB.CreateEventAsync(newEvent);

            // Assert
            Assert.IsTrue(eventID > 0, "The event ID should be greater than 0 if creation was successful.");
        }

        [TestMethod]
        public async Task TestCreateEventAsyncException()
        {
            // Arrange
            IEventDB eventDB = new EventDB_SQL();
            // Creating an event with a title that is null!
            IEvent newEvent = new Event(null, "TestDescription", DateTime.Today, TimeOnly.Parse("11:00:00"), "TestRoad 123", 10);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<SqlException>(async () =>
            {
                await eventDB.CreateEventAsync(newEvent);
            });
        }
        #endregion
    }
}
