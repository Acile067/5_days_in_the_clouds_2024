using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Levi9_competition.Interfaces;
using Levi9_competition.Controllers;
using Levi9_competition.Dtos.Player;
using Levi9_competition.Models;
using System.Threading.Tasks;
using Levi9_competition.Services;
using Levi9_competition.Dtos.Team;
using Levi9_competition.Mappers;

namespace Tests
{
    [TestFixture]
    public class TeamControllerTests
    {
        private Mock<TeamService> _teamServiceMock;
        private TeamController _teamController;

        [SetUp]
        public void Setup()
        {
            _teamServiceMock = new Mock<TeamService>(null);
            _teamController = new TeamController(_teamServiceMock.Object);
        }

        
    }
}
