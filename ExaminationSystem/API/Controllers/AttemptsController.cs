using ExaminationSystem.Features.Attempts.GetAttempts;
using ExaminationSystem.Features.Attempts.GetAttemptDetails;
using ExaminationSystem.BuildingBlocks.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [Route("api/student/attempts")]
    [ApiController]
    public class AttemptsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public AttemptsController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuizHistory(
            [FromQuery(Name = "quiz_id")] int? quizId,
            [FromQuery(Name = "diploma_id")] int? diplomaId,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "per_page")] int perPage = 20,
            CancellationToken cancellationToken = default)
        {
            int studentId = _currentUserService.UserId != 0 ? _currentUserService.UserId : 1;
            var query = new GetStudentAttemptQuery(studentId, quizId, diplomaId, page, perPage);
            var history = await _mediator.Send(query, cancellationToken);

            return Ok(history);
        }

        [HttpGet("{attemptId:int}")]
        public async Task<IActionResult> GetAttemptDetails(int attemptId, CancellationToken cancellationToken = default)
        {
            int studentId = _currentUserService.UserId != 0 ? _currentUserService.UserId : 1;
            var result = await _mediator.Send(new GetStudentAttemptDetailsQuery(studentId, attemptId), cancellationToken);
            return Ok(result);
        }

    }
}
