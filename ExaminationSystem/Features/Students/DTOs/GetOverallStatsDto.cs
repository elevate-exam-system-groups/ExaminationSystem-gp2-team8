using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Features.Quizzes.DTOS;

namespace ExaminationSystem.Features.Students.DTOs
{
    public class GetOverallStatsDto
    {
        public List<StudentEnrollmentDto>? StudentEnrollments { get; set; }
        public IEnumerable<StudentQuizAttemptsDto>? QuizAttempts { get; set; }
        public StudentStatsDto? OverallStats { get; set; }

    }
}
