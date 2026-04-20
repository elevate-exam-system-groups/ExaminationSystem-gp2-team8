using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Services
{
    public class AutoSubmitService : IAutoSubmitService
    {
        private readonly ExamAppDbContext _db;
        private readonly ILogger<AutoSubmitService> _logger;

        public AutoSubmitService(ExamAppDbContext db, ILogger<AutoSubmitService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task AutoSubmitAsync(Attempts attempt, CancellationToken cancellationToken = default)
        {
            // don't double-submit
            if (attempt.Attempt != AttemptStatus.InProgress)
            {
                return;
            }

            // Load answers that were received before the deadline
            var answersBeforeDeadline = await _db.StudentAnswers
                .Where(sa => sa.AttemptId == attempt.Id && sa.Attempt.StartTime <= attempt.Deadline)
                .ToListAsync(cancellationToken);

            //Load total question count for the quiz 
            var totalQuestions = await _db.Questions.CountAsync(q => q.QuizId == attempt.QuizId, cancellationToken);

            // ── Score: (correct before deadline / total questions) × 100 ─────
            var correctCount = answersBeforeDeadline.Count(a => a.SelectedOption.IsCorrect);

            var scorePct = totalQuestions > 0 ? (int)Math.Round((double)correctCount / totalQuestions * 100) : 0;

            // Load quiz pass threshold 
            var quiz = await _db.Quizzes.FindAsync([attempt.QuizId], cancellationToken);

            attempt.Attempt = AttemptStatus.TimeOut;
            attempt.SubmittedAt = DateTime.UtcNow;
            attempt.CorrectCount = correctCount;
            attempt.TotalQuestions = totalQuestions;
            attempt.ScorePct = scorePct;
            attempt.Passed = quiz is not null && scorePct >= quiz.PassScore;

            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Attempt {AttemptId} auto-submitted. Score: {Score}%, Passed: {Passed}",
                attempt.Id, scorePct, attempt.Passed);

        }
    }
}
