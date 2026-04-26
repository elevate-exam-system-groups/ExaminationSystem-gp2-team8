using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.StartQuiz.DTOS;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.StartQuiz.Queries
{
    public record GetActiveUserQuery(int UserId) : IRequest<Result<ActiveUserDto>>;

    public class GetActiveUserQueryHandler
        : IRequestHandler<GetActiveUserQuery, Result<ActiveUserDto>>
    {
        private readonly UserManager<User> _userManager;

        public GetActiveUserQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<ActiveUserDto>> Handle(
            GetActiveUserQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null || user.Status != UserStatus.Active)
                return Result<ActiveUserDto>.Fail("User not found or inactive");

            return Result<ActiveUserDto>.Success(new ActiveUserDto
            {
                Id = user.Id,
                IsActive = user.Status == UserStatus.Active
            });
        }
    }
}
