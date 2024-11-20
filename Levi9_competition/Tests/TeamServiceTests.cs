using Levi9_competition.Dtos.Team;
using Levi9_competition.Interfaces;
using Levi9_competition.Models;
using Levi9_competition.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class TeamServiceTests
    {
        private Mock<ITeamRepo> _teamRepoMock;
        private TeamService _teamService;

        [SetUp]
        public void Setup()
        {
            _teamRepoMock = new Mock<ITeamRepo>();
            _teamService = new TeamService(_teamRepoMock.Object);
        }


        [Test]
        public async Task CreateTeamAsync_TeamNameExists_ThrowsArgumentException()
        {
            var teamDto = new CreateTeamRequestDto
            {
                TeamName = "Existing Team",
                Players = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }
            };

            _teamRepoMock.Setup(repo => repo.TeamExisist(teamDto.TeamName)).ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _teamService.CreateTeamAsync(teamDto));
            Assert.AreEqual("Team name is already taken.", exception.Message);
        }

        [Test]
        public async Task CreateTeamAsync_PlayerDoesNotExist_ThrowsArgumentException()
        {
            var teamDto = new CreateTeamRequestDto
            {
                TeamName = "Test Team",
                Players = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }
            };

            _teamRepoMock.Setup(repo => repo.TeamExisist(teamDto.TeamName)).ReturnsAsync(false);
            _teamRepoMock.Setup(repo => repo.GetPlayersByGuidsAsync(It.IsAny<List<string>>())).ReturnsAsync(new List<Player>()); // Simulate no players found

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _teamService.CreateTeamAsync(teamDto));
            Assert.AreEqual("One or more players do not exist in the database.", exception.Message);
        }

        [Test]
        public async Task CreateTeamAsync_ValidRequest_ReturnsTeam()
        {
            var teamDto = new CreateTeamRequestDto
            {
                TeamName = "Test Team",
                Players = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }
            };

            var playersInDb = new List<Player>
            {
                new Player { Id = teamDto.Players[0] },
                new Player { Id = teamDto.Players[1] },
                new Player { Id = teamDto.Players[2] },
                new Player { Id = teamDto.Players[3] },
                new Player { Id = teamDto.Players[4] }
            };

            var team = new Team { Id = "1", TeamName = "Test Team", Players = playersInDb };

            _teamRepoMock.Setup(repo => repo.TeamExisist(teamDto.TeamName)).ReturnsAsync(false);
            _teamRepoMock.Setup(repo => repo.GetPlayersByGuidsAsync(It.IsAny<List<string>>())).ReturnsAsync(playersInDb);
            _teamRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<Team>())).ReturnsAsync(team);

            var result = await _teamService.CreateTeamAsync(teamDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(team.TeamName, result.TeamName);
            Assert.AreEqual(5, result.Players.Count);
        }

        [Test]
        public async Task CreateTeamAsync_PlayerAlreadyInTeam_ThrowsArgumentException()
        {
            var teamDto = new CreateTeamRequestDto
            {
                TeamName = "Test Team",
                Players = new List<string>
                {
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString()
                }
            };

            var playersInDb = new List<Player>
            {
                new Player { Id = teamDto.Players[0], Team = "ExistingTeamId" }, 
                new Player { Id = teamDto.Players[1] },
                new Player { Id = teamDto.Players[2] },
                new Player { Id = teamDto.Players[3] },
                new Player { Id = teamDto.Players[4] }
            };

            _teamRepoMock.Setup(repo => repo.TeamExisist(teamDto.TeamName)).ReturnsAsync(false);
            _teamRepoMock.Setup(repo => repo.GetPlayersByGuidsAsync(It.IsAny<List<string>>())).ReturnsAsync(playersInDb);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _teamService.CreateTeamAsync(teamDto));
            Assert.AreEqual("One or more players are already in another team.", exception.Message);
        }

        [Test]
        public async Task GetTeamByIdAsync_ValidId_ReturnsTeam()
        {
            var team = new Team
            {
                Id = Guid.NewGuid().ToString(),
                TeamName = "Existing Team",
                Players = new List<Player>
                {
                    new Player { Id = Guid.NewGuid().ToString() },
                    new Player { Id = Guid.NewGuid().ToString() }
                }
            };

            _teamRepoMock.Setup(repo => repo.GetByIdAsync(team.Id)).ReturnsAsync(team);

            var result = await _teamService.GetTeamByIdAsync(team.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(team.Id, result.Id);
            Assert.AreEqual(team.TeamName, result.TeamName);
        }
        [Test]
        public async Task GetTeamByIdAsync_InvalidId_ReturnsNull()
        {
            var invalidId = Guid.NewGuid().ToString();

            _teamRepoMock.Setup(repo => repo.GetByIdAsync(invalidId)).ReturnsAsync((Team?)null);

            var result = await _teamService.GetTeamByIdAsync(invalidId);

            Assert.IsNull(result);
        }
        [Test]
        public async Task CreateTeamAsync_InvalidNumberOfPlayers_ThrowsArgumentException()
        {
            var teamDto = new CreateTeamRequestDto
            {
                TeamName = "Test Team",
                Players = new List<string>
                {
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString() 
                }
            };

            var playersInDb = new List<Player>
            {
                new Player { Id = teamDto.Players[0] },
                new Player { Id = teamDto.Players[1] }
            };

            _teamRepoMock.Setup(repo => repo.TeamExisist(teamDto.TeamName)).ReturnsAsync(false);
            _teamRepoMock.Setup(repo => repo.GetPlayersByGuidsAsync(It.IsAny<List<string>>())).ReturnsAsync(playersInDb);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _teamService.CreateTeamAsync(teamDto));
            Assert.AreEqual("One or more players do not exist in the database.", exception.Message);
        }  
    }
}
