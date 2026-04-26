using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.GetAttemptResult
{
    public class ViewAttemptResultsHandler : IRequestHandler<ViewAttemptResults, ViewResultDTO>
    {

        IGeneralRepository<Domain.Entities.Attempts> _repository;
        public ViewAttemptResultsHandler(IGeneralRepository<Domain.Entities.Attempts> repository)
        {
            _repository = repository;
        }
        public async Task<ViewResultDTO> Handle(ViewAttemptResults request, CancellationToken cancellationToken)
        {
            var attempt = await _repository.Query()
                .Where(a => a.Id == request.id
                         && (a.Attempt == AttemptStatus.TimeOut || a.Attempt == AttemptStatus.Submit)
                         && a.UserId == request.studentId)
                .Select(a => new
                {
                    a.Id,
                    a.score,
                    QuizPassScore = a.Quiz.PassScore,
                    QuestionsCount = a.Quiz.Questions.Count,
                    StudentAnswers = a.StudentAnswers.Select(sa => new
                    {
                        sa.QuestionId,
                        sa.SelectedOption.OptionText,
                        sa.SelectedOption.IsCorrect,
                        CorrectAnswer = sa.Question.Options
                            .Where(o => o.IsCorrect)
                            .Select(o => o.OptionText)
                            .FirstOrDefault()
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (attempt is null)
                throw new ForbiddenException("results not available until submitted");

            var status = attempt.QuizPassScore <= attempt.score ? "Passed" : "Failed";

            var correctCount = attempt.StudentAnswers.Count(a => a.IsCorrect);

            var dto = new ViewResultDTO
            {
                score = attempt.score,
                status = status,
                TotalQuestions = attempt.QuestionsCount,
                CorrectCount = correctCount,
                Questions = attempt.StudentAnswers.Select(s => new QuestionAttemptsDTO
                {
                    QuestionId = s.QuestionId,
                    SelectedAnswer = s.OptionText,
                    CorrectAnswer = s.CorrectAnswer ?? "",
                    IsCorrect = s.IsCorrect
                }).ToList()
            };

            return dto;
        }

    }
}
