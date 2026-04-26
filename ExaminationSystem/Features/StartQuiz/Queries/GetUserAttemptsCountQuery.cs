using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using MediatR;
using ExaminationSystem.Features.StartQuiz.DTOS;
namespace ExaminationSystem.Features.StartQuiz.Queries
{
    public record GetUserAttemptsCountQuery(int UserId, int QuizId)
       : IRequest<Result<UserAttemptsCountDto>>;

    public class GetUserAttemptsCountQueryHandler
        : IRequestHandler<GetUserAttemptsCountQuery, Result<UserAttemptsCountDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserAttemptsCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserAttemptsCountDto>> Handle(
            GetUserAttemptsCountQuery request,
            CancellationToken cancellationToken)
        {
            var attemptsCount = await _unitOfWork.Repository<Attempts>()
                .GetAll(asNoTracking: true)
                .CountAsync(a =>
                    a.UserId == request.UserId &&
                    a.QuizId == request.QuizId,
                    cancellationToken);

            return Result<UserAttemptsCountDto>.Success(new UserAttemptsCountDto
            {
                AttemptsCount = attemptsCount
            });
        }
    }
}
