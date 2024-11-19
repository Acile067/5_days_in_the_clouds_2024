using Levi9_competition.Data;
using Levi9_competition.Dtos.Player;
using Levi9_competition.Interfaces;
using Levi9_competition.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Levi9_competition.Controllers
{
    [Route("players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPlayerRepo _playerRepo;
        public PlayerController(AppDbContext context, IPlayerRepo playerRepo)
        {
            _playerRepo = playerRepo;
            _context = context;
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreatePlayerRequestDto playerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _playerRepo.PlayerExisist(playerDto.Nickname))
            {
                return BadRequest("Player already exists");
            }

            var playerModel = playerDto.ToPlayerFromCreateDTO();

            await _playerRepo.CreateAsync(playerModel);

            return CreatedAtAction(nameof(GetById), new { id = playerModel.Id }, playerModel.ToPlayerDto());
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var player = await _playerRepo.GetByIdAsync(id);

            if (player == null)
            {
                return NotFound("Player not found");
            }

            return Ok(player.ToPlayerDto());
        }
        
    }
}
