using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using CA2.Data;

namespace CA2.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly FootballContext _context;

        public DatabaseHealthCheck(FootballContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Try to execute a simple query
                await _context.Teams.FirstOrDefaultAsync(cancellationToken);
                return HealthCheckResult.Healthy("Database is working correctly");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database is not responding", ex);
            }
        }
    }
} 