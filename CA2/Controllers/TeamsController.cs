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
            return await _context.Teams.Include(t => t.Players).ToListAsync();
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.TeamId == id);

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
                .Include(t => t.Players)
                .Where(t => t.League == leagueName)
                .ToListAsync();
        }

        // GET: api/Teams/country/{countryName}
        [HttpGet("country/{countryName}")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeamsByCountry(string countryName)
        {
            return await _context.Teams
                .Include(t => t.Players)
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
        public async Task<ActionResult<TeamStatistics>> GetTeamStatistics(int id)
        {
            var team = await _context.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.TeamId == id);

            if (team == null)
            {
                return NotFound();
            }

            var statistics = new TeamStatistics
            {
                TeamId = team.TeamId,
                Name = team.Name,
                TotalGoals = team.Players.Sum(p => p.Goals),
                TotalAssists = team.Players.Sum(p => p.Assists),
                AveragePlayerAge = team.Players.Any() ? team.Players.Average(p => p.Age) : 0,
                TotalAppearances = team.Players.Sum(p => p.Appearances),
                PlayerCount = team.Players.Count,
                GoalsPerGame = team.Players.Sum(p => p.Appearances) > 0 
                    ? (double)team.Players.Sum(p => p.Goals) / team.Players.Sum(p => p.Appearances) 
                    : 0
            };

            return statistics;
        }

        [HttpGet("league-statistics/{league}")]
        public async Task<ActionResult<LeagueStatistics>> GetLeagueStatistics(string league)
        {
            var teams = await _context.Teams
                .Include(t => t.Players)
                .Where(t => t.League == league)
                .ToListAsync();

            if (!teams.Any())
            {
                return NotFound();
            }

            var statistics = new LeagueStatistics
            {
                League = league,
                TotalTeams = teams.Count,
                TotalGoals = teams.Sum(t => t.Players.Sum(p => p.Goals)),
                TotalAssists = teams.Sum(t => t.Players.Sum(p => p.Assists)),
                AverageTeamAge = teams.Average(t => t.Players.Average(p => p.Age)),
                TotalPlayers = teams.Sum(t => t.Players.Count),
                GoalsPerGame = teams.Sum(t => t.Players.Sum(p => p.Appearances)) > 0
                    ? (double)teams.Sum(t => t.Players.Sum(p => p.Goals)) / teams.Sum(t => t.Players.Sum(p => p.Appearances))
                    : 0
            };

            return statistics;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Team>>> SearchTeams(
            [FromQuery] string? name,
            [FromQuery] string? league,
            [FromQuery] string? country,
            [FromQuery] int? minFoundedYear,
            [FromQuery] int? maxFoundedYear,
            [FromQuery] string? manager)
        {
            var query = _context.Teams.Include(t => t.Players).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(t => t.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(league))
            {
                query = query.Where(t => t.League == league);
            }

            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(t => t.Country == country);
            }

            if (minFoundedYear.HasValue)
            {
                query = query.Where(t => t.FoundedYear >= minFoundedYear.Value);
            }

            if (maxFoundedYear.HasValue)
            {
                query = query.Where(t => t.FoundedYear <= maxFoundedYear.Value);
            }

            if (!string.IsNullOrEmpty(manager))
            {
                query = query.Where(t => t.Manager.Contains(manager));
            }

            return await query.ToListAsync();
        }

        // POST: api/Teams
        [HttpPost]
        [Authorize(Policy = "AllowAll")]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeam), new { id = team.TeamId }, team);
        }

        // PUT: api/Teams/5
        [HttpPut("{id}")]
        [Authorize(Policy = "AllowAll")]
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
        [Authorize(Policy = "AllowAll")]
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