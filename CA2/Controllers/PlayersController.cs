using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CA2.Data;
using CA2.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CA2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly FootballContext _context;

        public PlayersController(FootballContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.Include(p => p.Team).ToListAsync();
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // GET: api/Players/team/5
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByTeam(int teamId)
        {
            return await _context.Players
                .Where(p => p.TeamId == teamId)
                .Include(p => p.Team)
                .ToListAsync();
        }

        // GET: api/Players/top-scorers
        [HttpGet("top-scorers")]
        public async Task<ActionResult<IEnumerable<Player>>> GetTopScorers()
        {
            return await _context.Players
                .Include(p => p.Team)
                .OrderByDescending(p => p.Goals)
                .Take(10)
                .ToListAsync();
        }

        // GET: api/Players/top-assists
        [HttpGet("top-assists")]
        public async Task<ActionResult<IEnumerable<Player>>> GetTopAssists()
        {
            return await _context.Players
                .Include(p => p.Team)
                .OrderByDescending(p => p.Assists)
                .Take(10)
                .ToListAsync();
        }

        // GET: api/Players/position/{position}
        [HttpGet("position/{position}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByPosition(string position)
        {
            return await _context.Players
                .Where(p => p.Position == position)
                .Include(p => p.Team)
                .ToListAsync();
        }

        // GET: api/Players/nationality/{nationality}
        [HttpGet("nationality/{nationality}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByNationality(string nationality)
        {
            return await _context.Players
                .Where(p => p.Nationality == nationality)
                .Include(p => p.Team)
                .ToListAsync();
        }

        // GET: api/Players/age-range?minAge={minAge}&maxAge={maxAge}
        [HttpGet("age-range")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByAgeRange([FromQuery] int minAge, [FromQuery] int maxAge)
        {
            return await _context.Players
                .Where(p => p.Age >= minAge && p.Age <= maxAge)
                .Include(p => p.Team)
                .ToListAsync();
        }

        // GET: api/Players/statistics/{id}
        [HttpGet("statistics/{id}")]
        public async Task<ActionResult<PlayerStatistics>> GetPlayerStatistics(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            var statistics = new PlayerStatistics
            {
                PlayerId = player.PlayerId,
                Name = player.Name,
                GoalsPerGame = player.Appearances > 0 ? (double)player.Goals / player.Appearances : 0,
                AssistsPerGame = player.Appearances > 0 ? (double)player.Assists / player.Appearances : 0,
                GoalContributionPerGame = player.Appearances > 0 ? (double)(player.Goals + player.Assists) / player.Appearances : 0,
                TotalGoalContributions = player.Goals + player.Assists
            };

            return statistics;
        }

        // GET: api/Players/top-contributors
        [HttpGet("top-contributors")]
        public async Task<ActionResult<IEnumerable<PlayerStatistics>>> GetTopContributors()
        {
            var players = await _context.Players
                .Select(p => new PlayerStatistics
                {
                    PlayerId = p.PlayerId,
                    Name = p.Name,
                    GoalsPerGame = p.Appearances > 0 ? (double)p.Goals / p.Appearances : 0,
                    AssistsPerGame = p.Appearances > 0 ? (double)p.Assists / p.Appearances : 0,
                    GoalContributionPerGame = p.Appearances > 0 ? (double)(p.Goals + p.Assists) / p.Appearances : 0,
                    TotalGoalContributions = p.Goals + p.Assists
                })
                .OrderByDescending(p => p.TotalGoalContributions)
                .Take(10)
                .ToListAsync();

            return players;
        }

        // GET: api/Players/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Player>>> SearchPlayers(
            [FromQuery] string? name,
            [FromQuery] int? minAge,
            [FromQuery] int? maxAge,
            [FromQuery] string? position,
            [FromQuery] string? nationality,
            [FromQuery] int? minGoals,
            [FromQuery] int? minAssists)
        {
            var query = _context.Players.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (minAge.HasValue)
            {
                query = query.Where(p => p.Age >= minAge.Value);
            }

            if (maxAge.HasValue)
            {
                query = query.Where(p => p.Age <= maxAge.Value);
            }

            if (!string.IsNullOrEmpty(position))
            {
                query = query.Where(p => p.Position == position);
            }

            if (!string.IsNullOrEmpty(nationality))
            {
                query = query.Where(p => p.Nationality == nationality);
            }

            if (minGoals.HasValue)
            {
                query = query.Where(p => p.Goals >= minGoals.Value);
            }

            if (minAssists.HasValue)
            {
                query = query.Where(p => p.Assists >= minAssists.Value);
            }

            return await query.ToListAsync();
        }

        // POST: api/Players
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer), new { id = player.PlayerId }, player);
        }

        // PUT: api/Players/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.PlayerId)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
} 