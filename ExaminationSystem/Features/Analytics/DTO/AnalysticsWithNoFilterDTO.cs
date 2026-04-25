using ExaminationSystem.Features.AnswerQuestion.DTOs;
using ExaminationSystem.Features.Attempts.DTOs;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Features.Quizzes.DTOS;

namespace ExaminationSystem.Features.Analytics.DTO
{
    public class AnalysticsWithNoFilterDTO
    {
        //pass rate by Quiz[]
        public IEnumerable<PassRateByQuizDTO> pass_rate_by_quiz { get; set; } = [];
        //average score by diploma
        public IEnumerable<AverageScorePerDiploamDTO> Avg_score_by_diploma { get; set; } = [];
        //attempts over time
        public IEnumerable<GetAttemptsOverTimeDTO> attempts_over_time { get; set; } = [];
        //top filed questions
        public IEnumerable<TopFailedQuestionDTO> top_failed_questions { get; set; } = [];
    }
}
