using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.AnswerQuestion.DTOs;
using ExaminationSystem.Infrastructure.Persistence;
using ExaminationSystem.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AnswerQuestion
{
    public class SubmitAnswerHandler : IRequestHandler<SubmitAnswerCommand, ApiResponse<SubmitAnswerResponseDto>>
    {
        private readonly ExamAppDbContext _db;
        private readonly IAutoSubmitService _autoSubmit;
        private readonly ILogger<SubmitAnswerHandler> _logger;

        public SubmitAnswerHandler(
            ExamAppDbContext db,
            IAutoSubmitService autoSubmit,
            ILogger<SubmitAnswerHandler> logger)
        {
            _db = db;
            _autoSubmit = autoSubmit;
            _logger = logger;
        }
        public async Task<ApiResponse<SubmitAnswerResponseDto>> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // Load attempt with its quiz 
            var attempt = await _db.Attempts
                .Include(a => a.Quiz)
                .FirstOrDefaultAsync(a => a.Id == request.AttemptId, cancellationToken);

            if (attempt is null)
                return ApiResponse<SubmitAnswerResponseDto>.Fail("Attempt not found.", statusCode: 404);

            //  Verify ownership — student must own this attempt 
            if (attempt.UserId != request.CurrentUserId)
                return ApiResponse<SubmitAnswerResponseDto>.Fail("Access denied. You do not own this attempt.", statusCode: 403);

            // Server-side timer check — enforce deadline on EVERY request ─ Per user story: if timer has elapsed → auto-submit → 410 Gone
            if (attempt.IsExpired)
            {
                await _autoSubmit.AutoSubmitAsync(attempt, cancellationToken);

                _logger.LogWarning("Answer submitted after deadline. Attempt {AttemptId} auto-submitted.", attempt.Id);

                return ApiResponse<SubmitAnswerResponseDto>.Fail("Time limit exceeded. Your attempt has been automatically submitted with answers on record.",statusCode: 410);
            }

            // Reject if attempt is already closed (submitted or timed out) 
            if (attempt.Attempt != AttemptStatus.InProgress)
                return ApiResponse<SubmitAnswerResponseDto>.Fail("This attempt has already been submitted and cannot be modified.",statusCode: 409);

            // Validate question belongs to this quiz
            var question = await _db.Questions
                .FirstOrDefaultAsync(q =>
                    q.Id == dto.QuestionId &&
                    q.QuizId == attempt.QuizId,
                    cancellationToken);

            if (question is null)
                return ApiResponse<SubmitAnswerResponseDto>.Fail($"Question {dto.QuestionId} does not belong to this quiz.",statusCode: 422);

            // load selected option belongs to this question 
            var selectedOption = await _db.Options
                .FirstOrDefaultAsync(o =>
                    o.Id == dto.SelectedOptionId &&
                    o.QuestionId == dto.QuestionId,
                    cancellationToken);

            if (selectedOption is null)
                return ApiResponse<SubmitAnswerResponseDto>.Fail($"Option {dto.SelectedOptionId} does not belong to question {dto.QuestionId}.",statusCode: 422);

            // UPSERT answer — re-answering overwrites previous answer
            //      Unique index on (AttemptId, QuestionId) in DB enforces one row per question.
            var existing = await _db.StudentAnswers
                .FirstOrDefaultAsync(sa =>
                    sa.AttemptId == attempt.Id &&
                    sa.QuestionId == dto.QuestionId,
                    cancellationToken);
            if (existing is null)
            {
                // INSERT — first time answering this question
                _db.StudentAnswers.Add(new StudentAnswer
                {
                    AttemptId = attempt.Id,
                    QuestionId = dto.QuestionId,
                    SelectedOptionId = dto.SelectedOptionId,
                    IsCorrect = selectedOption.IsCorrect,
                    AnsweredAt = DateTime.UtcNow,
                });

            }
            else
            {
                // UPDATE — student is changing their answer
                existing.SelectedOptionId = dto.SelectedOptionId;
                existing.IsCorrect = selectedOption.IsCorrect;
                existing.AnsweredAt = DateTime.UtcNow; // refresh timestamp
            }

            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Attempt {AttemptId} | Question {QuestionId} answered. Correct: {IsCorrect}",attempt.Id, dto.QuestionId, selectedOption.IsCorrect);

            // Calculate remaining seconds for client timer display 
            var secondsRemaining = (int)Math.Max(0, (attempt.Deadline - DateTime.UtcNow).TotalSeconds);

            return ApiResponse<SubmitAnswerResponseDto>.Ok(new SubmitAnswerResponseDto
            {
                Saved = true,
                QuestionId = dto.QuestionId,
                AttemptId = attempt.Id,
                SecondsRemaining = secondsRemaining,
            }, "Answer saved successfully.");
        }
    }
}
