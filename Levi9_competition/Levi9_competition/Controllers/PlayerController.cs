using Levi9_competition.Data;
using Levi9_competition.Dtos.Player;
using Levi9_competition.Mappers;
using Levi9_competition.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Levi9_competition.Controllers
{
    [Route("players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PlayerService _playerService;
        public PlayerController(AppDbContext context, PlayerService playerService)
        {
            _playerService = playerService;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var players = await _playerService.GetAllAsync();

            var playersDto = players.Select(s => s.ToPlayerDto());

            return Ok(playersDto);
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreatePlayerRequestDto playerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _playerService.PlayerExisist(playerDto.Nickname))
            {
                return BadRequest("Player already exists");
            }

            var playerModel = playerDto.ToPlayerFromCreateDTO();

            await _playerService.CreateAsync(playerModel);

            return Ok(playerModel.ToPlayerDto());
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var player = await _playerService.GetByIdAsync(id);

            if (player == null)
            {
                return NotFound("Player not found");
            }

            return Ok(player.ToPlayerDto());
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAllData()
        {
            await _playerService.DeleteAllDataAsync();
            return Ok();
        }

        [HttpPut]
        [Route("{player_id}/leave_team")]
        public async Task<IActionResult> LeaveTeam([FromRoute] string player_id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var player = await _playerService.GetByIdAsync(player_id);

            if (player == null)
            {
                return NotFound("Player not found");
            }

            player.Team = null;

            await _playerService.UpdateAsync(player);

            return Ok(player.ToPlayerDto());
        }

    }
}
