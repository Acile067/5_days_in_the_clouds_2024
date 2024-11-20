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
                Duration = 0 // Invalid duration
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
                Team2Id = teamId, // Same team
                Duration = 2
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _matchService.ProcessMatch(matchDto));
            Assert.AreEqual("Team1 and Team2 have same IDs.", ex.Message);
        }
    }
}
