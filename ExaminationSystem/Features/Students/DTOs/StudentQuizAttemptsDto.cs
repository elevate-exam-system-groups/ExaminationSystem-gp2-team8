namespace ExaminationSystem.Features.Students.DTOs
{
    public class StudentQuizAttemptsDto
    {
        public string? DiplomaTitle { get; set; }
        public string? QuizName { get; set; }
        public int Attempts { get; set; }
        public DateTime AttemptDate { get; set; }
    }
}