namespace CA2.Models
{
    public class PlayerStatistics
    {
        public int PlayerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double GoalsPerGame { get; set; }
        public double AssistsPerGame { get; set; }
        public double GoalContributionPerGame { get; set; }
        public int TotalGoalContributions { get; set; }
    }
} 