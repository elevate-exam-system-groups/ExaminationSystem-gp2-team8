namespace ExaminationSystem.Features.AdminStats.DTOs
{
    public class AdminStatsDto
    {
        
        public int TotalUsers { get; set; }

        
        public int ActiveUsersToday { get; set; }

       
        public int TotalDiplomas { get; set; }

       
        public int TotalQuizzes { get; set; }

        
        public int TotalAttempts { get; set; }

        
        // Overall pass rate across all submitted/timed-out attempts.
        // Formula: passed_attempts / total_completed_attempts × 100.
        // Returns 0 when no completed attempts exist.
      
        public double AvgPassRate { get; set; }

        // UTC timestamp this snapshot was generated (or served from cache)
        public DateTime GeneratedAt { get; set; }
    }
}
