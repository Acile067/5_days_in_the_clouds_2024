using Levi9_competition.Dtos.Team;
using Levi9_competition.Mappers;
using Levi9_competition.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Levi9_competition.Controllers
{
    [Route("teams")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamService _teamService;

        public TeamController(TeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamRequestDto teamDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var team = await _teamService.CreateTeamAsync(teamDto);
                return Ok(team.ToTeamDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
                return NotFound();

            return Ok(team.ToTeamDto());
        }
    }
}
