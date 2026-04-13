namespace ExaminationSystem.Features.Quizzes.DTOS
{
    public class UpdateQuizDto
    {
        public string Title { get; set; } = null!;
        public int DiplomaId { get; set; }
        public int DurationMinutes { get; set; }
        public int PassScore { get; set; } = 60;
        public int? MaxAttempts { get; set; }
        public string? Instructions { get; set; }
        //public string Status { get; set; } = string.Empty;
    }
}
