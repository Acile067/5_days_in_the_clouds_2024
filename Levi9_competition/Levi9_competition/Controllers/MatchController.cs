using Levi9_competition.Dtos.Match;
using Levi9_competition.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Levi9_competition.Controllers
{
    [Route("matches")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _matchService;

        public MatchController(MatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMatchRequestDto matchModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                bool result = await _matchService.ProcessMatch(matchModel);

                if (!result)
                    return StatusCode(500, "Failed to process the match.");

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
