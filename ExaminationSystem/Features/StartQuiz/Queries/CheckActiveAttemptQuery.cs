using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.StartQuiz.Queries
{
    public record CheckActiveAttemptQuery(int UserId, int QuizId)
    : IRequest<Result<bool>>;
    public class CheckActiveAttemptQueryHandler
    : IRequestHandler<CheckActiveAttemptQuery, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckActiveAttemptQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(
            CheckActiveAttemptQuery request,
            CancellationToken cancellationToken)
        {
            var hasActiveAttempt = await _unitOfWork.Repository<Attempts>()
                .GetAll(asNoTracking: true)
                .AnyAsync(a =>
                    a.UserId == request.UserId &&
                    a.QuizId == request.QuizId &&
                    a.Attempt == AttemptStatus.InProgress,
                    cancellationToken);

            return Result<bool>.Success(hasActiveAttempt);
        }
    }
}
