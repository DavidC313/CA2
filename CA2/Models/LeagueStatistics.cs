namespace CA2.Models
{
    public class LeagueStatistics
    {
        public string League { get; set; } = string.Empty;
        public int TotalTeams { get; set; }
        public int TotalGoals { get; set; }
        public int TotalAssists { get; set; }
        public double AverageTeamAge { get; set; }
        public int TotalPlayers { get; set; }
        public double GoalsPerGame { get; set; }
    }
} 