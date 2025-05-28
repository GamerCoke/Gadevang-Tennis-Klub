using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Gadevang_Tennis_Klub.Services.SQL;

namespace GTK_unittest
{
    [TestClass]
    public sealed class Test_Court
    {
        [TestMethod]
        public void Test_CreateCourtBookingAsync()
        {
            //Arrange
            ResetClassM.Reset();
            CourtDB_SQL db = new CourtDB_SQL();

            //Act
            ICourt original = new Court { ID = 10, Type = "Indendørs Tennis", Name = "Mark" };
            if (!db.CreateCourtAsync(original).Result)
                Assert.Fail();
            ICourt saved = db.GetCourtByIDAsync(10).Result;

            //Assert
            Assert.AreEqual(original.Name, saved.Name);
            Assert.AreEqual(original.Type, saved.Type);
            Assert.AreEqual(original.ID, saved.ID);
        }
        private class TestInvalidCourt : ICourt
        {
            public int ID { get; set; } = 3;
            public string Type { get; set; } = null;
            public string? Name { get; set; } = null;
        }
        [TestMethod]
        public void Test_InvalidCreateCourtAsync()
        {
            //Arrange
            ResetClassM.Reset();
            CourtDB_SQL db = new CourtDB_SQL();

            //Act
            ICourt original = new TestInvalidCourt();
            bool isCreated = true;
            try
            {
                isCreated = db.CreateCourtAsync(original).Result;
            }
            catch (Exception ex)
            {
                isCreated = false;
            }

            //Assert
            if (!isCreated)
                Assert.Fail();

        }
        [TestMethod]
        public void Test_UpdateCourtAsync()
        {
            //Arrange
            ResetClassM.Reset();
            CourtDB_SQL db = new CourtDB_SQL();

            //Act
            ICourt original = new Court (9, "Udendørs Tennis", "Mark");
            if (!db.UpdateCourtAsync(original).Result)
                Assert.Fail();
            ICourt updated = db.GetCourtByIDAsync(9).Result;

            //Assert
            Assert.AreEqual(original.ID, updated.ID);
            Assert.AreEqual(original.Type, updated.Type);
            Assert.AreEqual(original.Name, updated.Name);
        }
        [TestMethod]
        public void Test_DeleteCourtAsync()
        {
            //Arrange
            ResetClassM.Reset();
            CourtDB_SQL db = new CourtDB_SQL();

            //Act
            if (!db.DeleteCourtAsync(2).Result)
                Assert.Fail();
            int count = db.GetAllCourtsAsync().Result.Count;

            //Assert
            Assert.AreEqual(8, count);

        }
        [TestMethod]
        public void Test_GetAllCourtsAsync()
        {
            //Arrange
            ResetClassM.Reset();
            CourtDB_SQL db = new CourtDB_SQL();

            //Act
            var list = db.GetAllCourtsAsync().Result;

            //Assert
            Assert.AreEqual(9, list.Count);
        }
        [TestMethod]
        public void Test_GetCourtByIDAsync()
        {
            //Arrange
            ResetClassM.Reset();
            CourtDB_SQL db = new CourtDB_SQL();

            //Act
            var court = db.GetCourtByIDAsync(2).Result;

            //Assert
            Assert.AreEqual(2, court.ID);
            Assert.AreEqual("Udendørs Tennis", court.Type);
            Assert.AreEqual("Bane 2", court.Name);

        }

        [TestMethod]
        public void Test_GetCourtsByNameAsync()
        {
            //Arrange
            ResetClassM.Reset();
            CourtDB_SQL db = new CourtDB_SQL();

            //Act
            var list = db.GetCourtsByNameAsync("Bane 2").Result;

            //Assert
            Assert.AreEqual(1, list.Count);

            Assert.AreEqual(2, list[0].ID);
            Assert.AreEqual("Udendørs Tennis", list[0].Type);
            Assert.AreEqual("Bane 2", list[0].Name);
        }
        [TestMethod]
        public void Test_GetCourtsByTypeAsync()
        {
            //Arrange
            ResetClassM.Reset();
            CourtDB_SQL db = new CourtDB_SQL();

            //Act
            var list = db.GetCourtsByTypeAsync("Udendørs Tennis").Result;

            //Assert
            Assert.AreEqual(5, list.Count);

            Assert.AreEqual(1, list[0].ID);
            Assert.AreEqual("Udendørs Tennis", list[0].Type);
            Assert.AreEqual("Bane 1", list[0].Name);

            Assert.AreEqual(2, list[1].ID);
            Assert.AreEqual("Udendørs Tennis", list[1].Type);
            Assert.AreEqual("Bane 2", list[1].Name);
        }
    }
}
