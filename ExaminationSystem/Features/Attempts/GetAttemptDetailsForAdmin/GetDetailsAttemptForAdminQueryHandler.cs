using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Features.Attempts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.GetAttemptDetailsForAdmin
{
    public class GetDetailsAttemptForAdminQueryHandler : IRequestHandler<GetDetailsAttemptByIdForAdminQuery, AttemptSummaryDTO?>
    {
        private readonly IGeneralRepository<Domain.Entities.Attempts> _repository;

        public GetDetailsAttemptForAdminQueryHandler(IGeneralRepository<Domain.Entities.Attempts> repository)
        {
            _repository = repository;
        }
        public async Task<AttemptSummaryDTO?> Handle(
            GetDetailsAttemptByIdForAdminQuery request,
            CancellationToken cancellationToken)
        {
            var attempt = await _repository.GetByIdAsync(request.Id);

            if (attempt == null)
                throw new NotFoundException($"Attempt with ID {request.Id} not found.");

            return new AttemptSummaryDTO
            {
                attemptedId = attempt.Id,
                studentId = attempt.UserId,
                QuizId = attempt.QuizId,
                submittedAt= attempt.SubmittedAt,
                status= attempt.Attempt.ToString(),
                score= attempt.score
            };
        }
    }
}
