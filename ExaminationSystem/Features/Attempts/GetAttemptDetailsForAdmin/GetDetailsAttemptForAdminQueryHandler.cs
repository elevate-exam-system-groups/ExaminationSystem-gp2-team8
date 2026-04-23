using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.GetAttemptDetailsForAdmin
{
    public class GetDetailsAttemptForAdminQueryHandler : IRequestHandler<GetDetailsAttemptByIdForAdminQuery, AttemptAdminSummaryDTO?>
    {
        private readonly IGeneralRepository<Domain.Entities.Attempts> _repository;

        public GetDetailsAttemptForAdminQueryHandler(IGeneralRepository<Domain.Entities.Attempts> repository)
        {
            _repository = repository;
        }
        public Task<AttemptAdminSummaryDTO?> Handle(GetDetailsAttemptByIdForAdminQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query().Where(a => a.Id == request.Id)
                .Select(a => new AttemptAdminSummaryDTO
                {
                    attemptedId = a.Id,
                    studentId = a.User.Id,
                    quiztitle = a.Quiz.Title,
                    score = a.score,
                    status = a.Attempt.ToString(),
                    submittedAt = a.SubmittedAt,
                    Questions = a.StudentAnswers.Select(sa => new QuestionAttemptDTO
                    {
                        QuestionId = sa.QuestionId,
                        QuestionText = sa.Question.QuestionText,

                        SelectedOptionId = sa.SelectedOptionId,
                        SelectedOptionText = sa.SelectedOption.OptionText,

                        //CorrectAnswer = sa.Question.,
                        IsCorrect = sa.IsCorrect
                    }).ToList()

                    // Map properties from the Attempts entity to the AttemptAdminSummaryDTO
                }).FirstOrDefaultAsync();

            if(query == null)
            {
                throw new NotFoundException($"Attempt with ID {request.Id} not found.");
            }

            return query;
        }
    }
}
