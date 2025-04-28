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
    public class TeamsController : ControllerBase
    {
        private readonly FootballContext _context;

        public TeamsController(FootballContext context)
        {
            _context = context;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            return await _context.Teams.ToListAsync();
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            return team;
        }

        // GET: api/Teams/league/{leagueName}
        [HttpGet("league/{leagueName}")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeamsByLeague(string leagueName)
        {
            return await _context.Teams
                .Where(t => t.League == leagueName)
                .ToListAsync();
        }

        // GET: api/Teams/country/{countryName}
        [HttpGet("country/{countryName}")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeamsByCountry(string countryName)
        {
            return await _context.Teams
                .Where(t => t.Country == countryName)
                .ToListAsync();
        }

        // GET: api/Teams/{id}/top-scorers
        [HttpGet("{id}/top-scorers")]
        public async Task<ActionResult<IEnumerable<Player>>> GetTopScorers(int id)
        {
            var team = await _context.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.TeamId == id);

            if (team == null)
            {
                return NotFound();
            }

            return team.Players
                .OrderByDescending(p => p.Goals)
                .ToList();
        }

        // GET: api/Teams/{id}/statistics
        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<object>> GetTeamStatistics(int id)
        {
            var team = await _context.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.TeamId == id);

            if (team == null)
            {
                return NotFound();
            }

            var statistics = new
            {
                TotalGoals = team.Players.Sum(p => p.Goals),
                TotalAssists = team.Players.Sum(p => p.Assists),
                TotalAppearances = team.Players.Sum(p => p.Appearances),
                AverageAge = team.Players.Average(p => p.Age),
                PlayerCount = team.Players.Count,
                TopScorer = team.Players.OrderByDescending(p => p.Goals).FirstOrDefault()?.Name,
                MostAssists = team.Players.OrderByDescending(p => p.Assists).FirstOrDefault()?.Name
            };

            return statistics;
        }

        // POST: api/Teams
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeam", new { id = team.TeamId }, team);
        }

        // PUT: api/Teams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, Team team)
        {
            if (id != team.TeamId)
            {
                return BadRequest();
            }

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.TeamId == id);
        }
    }
} 