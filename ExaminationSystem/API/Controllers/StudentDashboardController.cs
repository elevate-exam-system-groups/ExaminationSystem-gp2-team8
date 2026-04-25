using ExaminationSystem.BuildingBlocks.Helpers;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.BuildingBlocks.Exceptions;
using ExaminationSystem.Features.Students.ViewDashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [Route("api/student/dashboard")]
    [ApiController]
    public class StudentDashboardController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public StudentDashboardController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetStudentDashboard(CancellationToken cancellationToken)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new ViewDashboardQuery(GetValidatedStudentId()), cancellationToken),
                Ok);
        }

        private int GetValidatedStudentId()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedException("Authentication is required.");
            }

            if (!User.IsInRole("Student"))
            {
                throw new ForbiddenException("Only students can access this dashboard.");
            }

            var studentId = _currentUserService.UserId;
            if (studentId <= 0)
            {
                throw new UnauthorizedException("Invalid token.");
            }

            return studentId;
        }
    }
}
