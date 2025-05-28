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
        private class TestIsvalidCourt : ICourt
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
            ICourt original = new TestIsvalidCourt();
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
            if (isCreated)
                Assert.Fail();

        }
        [TestMethod]
        public void Test_UpdateCourtAsync()
        {
            //Arrange
            ResetClassM.Reset();
            CourtDB_SQL db = new CourtDB_SQL();

            //Act
            ICourt original = new Court(0, "Indendørs Tennis", "Mark");
            if (!db.UpdateCourtAsync(original).Result)
                Assert.Fail();
            ICourt updated = db.GetCourtByIDAsync(6).Result;

            //Assert
            Assert.AreEqual(original.Name, updated.Name);
            Assert.AreEqual(original.Type, updated.Type);
            Assert.AreEqual(original.ID, updated.ID);
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
            Assert.AreEqual(5, list.Count);

            Assert.AreEqual(1, list[0].ID);
            Assert.AreEqual("Udendørs Tennis", list[0].Type);
            Assert.AreEqual("Bane 1", list[0].Name);

            Assert.AreEqual(2, list[1].ID);
            Assert.AreEqual("Udendørs Tennis", list[1].Type);
            Assert.AreEqual("Bane 2", list[1].Name);

            Assert.AreEqual(3, list[2].ID);
            Assert.AreEqual("Indendørs Tennis", list[2].Type);
            Assert.AreEqual("", list[2].Name);

            Assert.AreEqual(4, list[3].ID);
            Assert.AreEqual("Indendørs Tennis", list[3].Type);
            Assert.AreEqual("Vores 2. IndendørsTennisBane", list[3].Name);

            Assert.AreEqual(5, list[4].ID);
            Assert.AreEqual("Udendørs Tennis", list[4].Type);
            Assert.AreEqual("", list[4].Name);

            Assert.AreEqual(6, list[5].ID);
            Assert.AreEqual("Indendørs Tennis", list[5].Type);
            Assert.AreEqual("Mark", list[5].Name);
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
            var list = db.GetCourtsByNameAsync("2").Result;

            //Assert
            Assert.AreEqual(2, list.Count);

            Assert.AreEqual(0, list[0].ID);
            Assert.AreEqual("Udendørs Tennis", list[0].Type);
            Assert.AreEqual("Bane 2", list[0].Name);

            Assert.AreEqual(1, list[1].ID);
            Assert.AreEqual("Indendørs Tennis", list[1].Type);
            Assert.AreEqual("Vores 2. IndendørsTennisBane", list[1].Name);
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
            Assert.AreEqual(3, list.Count);

            Assert.AreEqual(1, list[0].ID);
            Assert.AreEqual("Udendørs Tennis", list[0].Type);
            Assert.AreEqual("Bane 1", list[0].Name);

            Assert.AreEqual(2, list[1].ID);
            Assert.AreEqual("Udendørs Tennis", list[1].Type);
            Assert.AreEqual("Bane 2", list[1].Name);


            Assert.AreEqual(5, list[2].ID);
            Assert.AreEqual("Udendørs Tennis", list[2].Type);
            Assert.AreEqual("", list[2].Name);
        }
    }
}
