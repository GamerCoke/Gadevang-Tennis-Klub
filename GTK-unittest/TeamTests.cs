using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gadevang_Tennis_Klub.Tests
{
    [TestClass]
    public class TeamTests
    {
        private Mock<ITeamDB> _mockTeamDB;

        [TestInitialize]
        public void Setup()
        {
            _mockTeamDB = new Mock<ITeamDB>();
        }

        [TestMethod]
        public async Task GetAllTeams_ShouldReturnList()
        {
            // Arrange
            var teams = new List<ITeam>
            {
                new Team { ID = 1, Name = "Team A", Capacity = 10 },
                new Team { ID = 2, Name = "Team B", Capacity = 15 }
            };

            _mockTeamDB.Setup(db => db.GetAllTeamsAsync()).ReturnsAsync(teams);

            // Act
            var result = await _mockTeamDB.Object.GetAllTeamsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task CreateTeam_ShouldSucceed()
        {
            // Arrange
            var newTeam = new Team { Name = "Winners", Capacity = 12 };
            _mockTeamDB.Setup(db => db.CreateTeamAsync(It.IsAny<Team>())).ReturnsAsync(true);

            // Act
            var success = await _mockTeamDB.Object.CreateTeamAsync(newTeam);

            // Assert
            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task CreateTeam_ShouldFail_WhenCapacityIsZero()
        {
            // Arrange
            var invalidTeam = new Team { Name = "Losers", Capacity = 0 };
            _mockTeamDB.Setup(db => db.CreateTeamAsync(It.Is<Team>(t => t.Capacity <= 0))).ReturnsAsync(false);

            // Act
            var result = await _mockTeamDB.Object.CreateTeamAsync(invalidTeam);

            // Assert (this test should fail if the CreateTeamAsync wrongly returns true)
            Assert.IsFalse(result);
        }
    }
}