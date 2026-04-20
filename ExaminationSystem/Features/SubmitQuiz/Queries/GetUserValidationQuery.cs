using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.SubmitQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.SubmitQuiz.Queries
{
    public record GetUserValidationQuery(int UserId)
        : IRequest<Result<UserValidationResultDto>>;

    public class GetUserValidationQueryHandler
        : IRequestHandler<GetUserValidationQuery, Result<UserValidationResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserValidationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserValidationResultDto>> Handle(
            GetUserValidationQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<User>()
                .GetByIdAsync(request.UserId);

            if (user == null)
                return Result<UserValidationResultDto>.Fail("User not found");

            if (user.Status != UserStatus.Active)
                return Result<UserValidationResultDto>.Fail("User is not active");

            return Result<UserValidationResultDto>.Success(new UserValidationResultDto
            {
                UserId = user.Id,
                IsFound = true,
                IsActive = true
            });
        }
    }
}