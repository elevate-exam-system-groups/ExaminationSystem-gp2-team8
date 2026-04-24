using ExaminationSystem.Features.AnswerQuestion;

namespace ExaminationSystem.Features.Attempts.DTOs
{
    public class AttemptForAdminDetailsDTO
    {
        public int attemptedId { get; set; }
        public int studentId { get; set; }

        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = default!;
        public int QuizDuration { get; set; }
        public float score { get; set; }
        public string status { get; set; } = default!;
        public DateTime submittedAt { get; set; }
        public IEnumerable<GetAnswerDetailsDTO> Questions { get; set; } = [];
    }
}
