using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CA2.Data;
using CA2.Models;
using System.Linq;

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

        // POST: api/Players
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { id = player.PlayerId }, player);
        }

        // PUT: api/Players/5
        [HttpPut("{id}")]
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