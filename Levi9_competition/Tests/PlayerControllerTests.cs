using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Levi9_competition.Interfaces;
using Levi9_competition.Controllers;
using Levi9_competition.Dtos.Player;
using Levi9_competition.Models;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class PlayerControllerTests
    {
        private Mock<IPlayerRepo> _mockPlayerRepo;
        private PlayerController _playerController;
        [SetUp]
        public void SetUp()
        {
            _mockPlayerRepo = new Mock<IPlayerRepo>();
            _playerController = new PlayerController(null, _mockPlayerRepo.Object);
        }

        [Test]
        public async Task CreateOnePlayerSuccesTest()
        {
            var playerDto = new CreatePlayerRequestDto
            {
                Nickname = "Player1",
            };
            var player = new Player
            {
                Id = "b9d3df62-7b9c-472e-b97f-59e2d65c1156",
                Nickname = "Player1",
                Elo = 1500,
                HoursPlayed = 100,
                Wins = 11,
                Losses = 2,
                RatingAdjustment = 50,
                Team = null
            };
            _mockPlayerRepo.Setup(repo => repo.PlayerExisist(playerDto.Nickname))
                .ReturnsAsync(false);
            _mockPlayerRepo.Setup(repo => repo.CreateAsync(It.IsAny<Player>()))
                .ReturnsAsync(player);

            var result = await _playerController.Create(playerDto);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult.");

            var returnValue = okResult?.Value as PlayerDto;
            Assert.IsNotNull(returnValue, "Expected PlayerDto.");
            Assert.AreEqual(player.Nickname, returnValue?.Nickname);

            _mockPlayerRepo.Verify(repo => repo.PlayerExisist(playerDto.Nickname), Times.Once);
            _mockPlayerRepo.Verify(repo => repo.CreateAsync(It.IsAny<Player>()), Times.Once);

        }
        [Test]
        public async Task CreatePlayerWithDuplicateNicknameTest()
        {
            var playerDto = new CreatePlayerRequestDto
            {
                Nickname = "Player1"
            };

            _mockPlayerRepo.Setup(repo => repo.PlayerExisist(playerDto.Nickname))
                .ReturnsAsync(true);

            var result = await _playerController.Create(playerDto);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult, "The result is not a BadRequestObjectResult.");
            Assert.AreEqual(400, badRequestResult?.StatusCode);

            var errorMessage = badRequestResult?.Value as string;
            Assert.AreEqual("Player already exists", errorMessage, "Error message is not as expected.");

            _mockPlayerRepo.Verify(repo => repo.PlayerExisist(playerDto.Nickname), Times.Once);
            _mockPlayerRepo.Verify(repo => repo.CreateAsync(It.IsAny<Player>()), Times.Never);
        }
        [Test]
        public async Task GetAllPlayersSuccessTest()
        {

            var players = new List<Player>
                {
                    new Player { Id = "1", Nickname = "Player1", Elo = 1500, HoursPlayed = 100 },
                    new Player { Id = "2", Nickname = "Player2", Elo = 1600, HoursPlayed = 200 }
                };

            _mockPlayerRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(players);

            var result = await _playerController.GetAll();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var playersDto = okResult?.Value as IEnumerable<PlayerDto>;
            Assert.AreEqual(2, playersDto.Count());

            _mockPlayerRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
        [Test]
        public async Task GetPlayerByIdSuccessTest()
        {
            var player = new Player { Id = "1", Nickname = "Player1", Elo = 1500, HoursPlayed = 100 };
            _mockPlayerRepo.Setup(repo => repo.GetByIdAsync("1")).ReturnsAsync(player);

            var result = await _playerController.GetById("1");

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var playerDto = okResult?.Value as PlayerDto;
            Assert.AreEqual("Player1", playerDto?.Nickname);

            _mockPlayerRepo.Verify(repo => repo.GetByIdAsync("1"), Times.Once);
        }
        [Test]
        public async Task GetPlayerByIdNotFoundTest()
        {
            _mockPlayerRepo.Setup(repo => repo.GetByIdAsync("999")).ReturnsAsync((Player)null);

            var result = await _playerController.GetById("999");

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult?.StatusCode);
            Assert.AreEqual("Player not found", notFoundResult?.Value);

            _mockPlayerRepo.Verify(repo => repo.GetByIdAsync("999"), Times.Once);
        }
        [Test]
        public async Task CreatePlayerBadRequestTest()
        {
            var playerDto = new CreatePlayerRequestDto
            {

            };

            _playerController.ModelState.AddModelError("Nickname", "Nickname is required.");

            var result = await _playerController.Create(playerDto);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult?.StatusCode);
        }
    }
}
