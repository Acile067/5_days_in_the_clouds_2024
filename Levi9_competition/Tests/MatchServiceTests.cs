using Levi9_competition.Dtos.Match;
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
    public class MatchServiceTests
    {
        private Mock<ITeamRepo> _teamRepoMock;
        private Mock<IMatchRepo> _matchRepoMock;
        private MatchService _matchService;

        [SetUp]
        public void Setup()
        {
            _teamRepoMock = new Mock<ITeamRepo>();
            _matchRepoMock = new Mock<IMatchRepo>();
            _matchService = new MatchService(_teamRepoMock.Object, _matchRepoMock.Object);
        }

        [Test]
        public void ProcessMatch_InvalidDuration_ThrowsArgumentException()
        {
            var matchDto = new CreateMatchRequestDto
            {
                Team1Id = Guid.NewGuid().ToString(),
                Team2Id = Guid.NewGuid().ToString(),
                Duration = 0 
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _matchService.ProcessMatch(matchDto));
            Assert.AreEqual("Duration must be at least 1 hour.", ex.Message);
        }

        [Test]
        public void ProcessMatch_SameTeamIds_ThrowsArgumentException()
        {
            var teamId = Guid.NewGuid().ToString();
            var matchDto = new CreateMatchRequestDto
            {
                Team1Id = teamId,
                Team2Id = teamId, 
                Duration = 2
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _matchService.ProcessMatch(matchDto));
            Assert.AreEqual("Team1 and Team2 have same IDs.", ex.Message);
        }
        [Test]
        public void ProcessMatch_NullTeamId_ThrowsArgumentException()
        {
            var matchDto = new CreateMatchRequestDto
            {
                Team1Id = null,
                Team2Id = Guid.NewGuid().ToString(),
                Duration = 1
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _matchService.ProcessMatch(matchDto));
            Assert.AreEqual("Team1Id and Team2Id must be valid.", ex.Message);
        }
        [Test]
        public void ProcessMatch_NonExistentTeam_ThrowsArgumentException()
        {
            var team1Id = Guid.NewGuid().ToString();
            var team2Id = Guid.NewGuid().ToString();
            var matchDto = new CreateMatchRequestDto
            {
                Team1Id = team1Id,
                Team2Id = team2Id,
                Duration = 2
            };

            _teamRepoMock.Setup(repo => repo.GetByIdAsync(team1Id)).ReturnsAsync((Team)null);
            _teamRepoMock.Setup(repo => repo.GetByIdAsync(team2Id)).ReturnsAsync(new Team());

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _matchService.ProcessMatch(matchDto));
            Assert.AreEqual("One or both teams do not exist.", ex.Message);
        }

        [Test]
        public async Task ProcessMatch_Team1Wins_UpdatesEloAndStats()
        {
            var team1 = new Team
            {
                Id = Guid.NewGuid().ToString(),
                Players = new List<Player>
            {
                new Player { Elo = 1000, HoursPlayed = 500, Wins = 10, Losses = 5 }
            }
            };
            var team2 = new Team
            {
                Id = Guid.NewGuid().ToString(),
                Players = new List<Player>
            {
                new Player { Elo = 1100, HoursPlayed = 400, Wins = 20, Losses = 15 }
            }
            };
            var matchDto = new CreateMatchRequestDto
            {
                Team1Id = team1.Id,
                Team2Id = team2.Id,
                WinningTeamId = team1.Id,
                Duration = 2
            };

            _teamRepoMock.Setup(repo => repo.GetByIdAsync(team1.Id)).ReturnsAsync(team1);
            _teamRepoMock.Setup(repo => repo.GetByIdAsync(team2.Id)).ReturnsAsync(team2);

            bool result = await _matchService.ProcessMatch(matchDto);

            Assert.IsTrue(result);

            var player1 = team1.Players.First();
            var player2 = team2.Players.First();

            Assert.AreEqual(502, player1.HoursPlayed);
            Assert.AreEqual(40, player1.RatingAdjustment);
            Assert.AreEqual(11, player1.Wins);

            Assert.AreEqual(402, player2.HoursPlayed);
            Assert.AreEqual(50, player2.RatingAdjustment);
            Assert.AreEqual(16, player2.Losses);
        }
    }
}
