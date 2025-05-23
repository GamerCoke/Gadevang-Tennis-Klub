using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Models;
using Gadevang_Tennis_Klub.Services.SQL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTK_unittest.Kasper
{
    [TestClass]
    public class Test_TrainerDB_SQL
    {
        [TestMethod]
        public void Test_GetAllTrainersAsync()
        {
            //Arrange
            ResetClass.Reset();
            TrainerDB_SQL db = new TrainerDB_SQL();

            //Act
            var list = db.GetAllTrainersAsync().Result;

            //Assert
            Assert.AreEqual(3, list.Count);

            Assert.AreEqual(0, list[0].Id);
            Assert.AreEqual("Kenneth", list[0].Name);
            Assert.AreEqual("12332165", list[0].Phone);
            Assert.AreEqual("Kenneth@mail.dk", list[0].Email);
            Assert.AreEqual(null, list[0].Image);

            Assert.AreEqual(1, list[1].Id);
            Assert.AreEqual("Smith", list[1].Name);
            Assert.AreEqual("+45 56 56 56 56", list[1].Phone);
            Assert.AreEqual("Smith@mail.dk", list[1].Email);
            Assert.AreEqual(null, list[1].Image);

            Assert.AreEqual(2, list[2].Id);
            Assert.AreEqual("Daffy", list[2].Name);
            Assert.AreEqual("+45 56 56 56 56", list[2].Phone);
            Assert.AreEqual("Daffy@mail.us", list[2].Email);
            Assert.AreEqual(null, list[2].Image);

        }
        [TestMethod]
        public void Test_GetTrainerByIDAsync()
        {
            //Arrange
            ResetClass.Reset();
            TrainerDB_SQL db = new TrainerDB_SQL();

            //Act
            ITrainer saved = db.GetTrainerByIDAsync(2).Result;

            //Assert
            Assert.AreEqual(2, saved.Id);
            Assert.AreEqual("Daffy", saved.Name);
            Assert.AreEqual("+45 56 56 56 56", saved.Phone);
            Assert.AreEqual("Daffy@mail.us", saved.Email);
            Assert.AreEqual(null, saved.Image);
        }
        [TestMethod]
        public void Test_CreateTrainerAsync()
        {
            //Arrange
            ResetClass.Reset();
            TrainerDB_SQL db = new TrainerDB_SQL();

            //Act
            ITrainer original = new Trainer(-1, "sdfg", "regcs", "15246", null);
            if (!db.CreateTrainerAsync(original).Result)
                Assert.Fail();
            ITrainer saved = db.GetTrainerByIDAsync(3).Result;

            //Assert
            Assert.AreEqual(original.Name, saved.Name);
            Assert.AreEqual(original.Phone, saved.Phone);
            Assert.AreEqual(original.Email, saved.Email);
            Assert.AreEqual(original.Image, saved.Image);

        }
        [TestMethod]
        public void Test_DeleteTrainerAsync()
        {
            //Arrange
            ResetClass.Reset();
            TrainerDB_SQL db = new TrainerDB_SQL();

            //Act
            if (!db.DeleteTrainerAsync(0).Result)
                Assert.Fail();
            var list = db.GetAllTrainersAsync().Result;

            //Assert
            Assert.AreEqual(2, list.Count);
        }
        [TestMethod]
        public void Test_UpdateTrainerAsync()
        {
            //Arrange
            ResetClass.Reset();
            TrainerDB_SQL db = new TrainerDB_SQL();

            //Act
            ITrainer original = new Trainer(1, "dave", "no", "nope", null);
            if (!db.UpdateTrainerAsync(original).Result)
                Assert.Fail();
            ITrainer updated = db.GetTrainerByIDAsync(1).Result;

            //Assert
            Assert.AreEqual(original.Name, updated.Name);
            Assert.AreEqual(original.Phone, updated.Phone);
            Assert.AreEqual(original.Email, updated.Email);
            Assert.AreEqual(original.Image, updated.Image);
        }
    }
}
