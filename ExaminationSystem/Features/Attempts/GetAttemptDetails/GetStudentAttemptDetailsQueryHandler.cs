using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Attempts.DTOs;
using ExaminationSystem.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.GetAttemptDetails
{
    public class GetStudentAttemptDetailsQueryHandler : IRequestHandler<GetStudentAttemptDetailsQuery, AttemptResultDetailDto>
    {
        private readonly ExamAppDbContext _dbContext;

        public GetStudentAttemptDetailsQueryHandler(ExamAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AttemptResultDetailDto> Handle(GetStudentAttemptDetailsQuery request, CancellationToken cancellationToken)
        {
            var attempt = await _dbContext.Attempts
                .AsNoTracking()
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.Questions)
                        .ThenInclude(q => q.Options)
                .Include(a => a.StudentAnswers)
                    .ThenInclude(answer => answer.SelectedOption)
                .FirstOrDefaultAsync(a => a.Id == request.AttemptId && a.UserId == request.StudentId, cancellationToken);

            if (attempt == null)
            {
                throw new NotFoundException("Attempt Not Found");
            }

            if (attempt.Attempt == AttemptStatus.InProgress)
            {
                throw new ForbiddenException("Results are not available until the attempt is submitted");
            }

            var perQuestion = attempt.Quiz.Questions
                .OrderBy(question => question.OrderIndex)
                .Select(question =>
                {
                    var studentAnswer = attempt.StudentAnswers.FirstOrDefault(answer => answer.QuestionId == question.Id);
                    var correctOption = question.Options.FirstOrDefault(option => option.IsCorrect);

                    return new AttemptQuestionDetailDto(
                        question.Id,
                        question.QuestionText,
                        studentAnswer?.SelectedOption.OptionText,
                        correctOption?.OptionText ?? string.Empty,
                        studentAnswer?.SelectedOption.IsCorrect ?? false,
                        question.Explanation
                    );
                })
                .ToList();

            var correctCount = perQuestion.Count(question => question.IsCorrect);

            return new AttemptResultDetailDto(
                attempt.Id,
                attempt.Quiz.Title,
                attempt.score,
                attempt.score >= attempt.Quiz.PassScore,
                attempt.Attempt.ToString(),
                attempt.SubmittedAt,
                perQuestion.Count,
                correctCount,
                perQuestion
            );
        }
    }
}
