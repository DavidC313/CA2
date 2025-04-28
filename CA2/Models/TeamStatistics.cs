namespace CA2.Models
{
    public class TeamStatistics
    {
        public int TeamId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalGoals { get; set; }
        public int TotalAssists { get; set; }
        public double AveragePlayerAge { get; set; }
        public int TotalAppearances { get; set; }
        public int PlayerCount { get; set; }
        public double GoalsPerGame { get; set; }
    }
} 