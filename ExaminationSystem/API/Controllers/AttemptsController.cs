using ExaminationSystem.BuildingBlocks.Helpers;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Features.Attempt;
using ExaminationSystem.Features.Attempts.GetAttemptDetails;
using ExaminationSystem.Features.Attempts.GetAttempts;
using MediatR;
using Microsoft.AspNetCore.Http;
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
            [FromQuery] int? quizId, 
            [FromQuery] int? diplomaId, 
            [FromQuery] int page= 1, 
            [FromQuery] int perPage= 20)
        {
            int studentId = _currentUserService.UserId != 0 ? _currentUserService.UserId : 1;
            return await ControllerHelper.ExecuteAsync(
                async () =>
                {
                    var query = new GetStudentAttemptQuery(studentId, quizId, diplomaId, page, perPage);
                    return await _mediator.Send(query);
                },
                history => history.Data.Count == 0 ? NotFound() : Ok(history));
        }

        [HttpGet("{attemptId:int}")]
        public async Task<IActionResult> GetAttemptDetails(int attemptId)
        {
            int studentId = _currentUserService.UserId != 0 ? _currentUserService.UserId : 1;
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new GetStudentAttemptDetailsQuery(studentId, attemptId)),
                Ok);
        }


        [HttpGet("{attemptid}/results")]
        public async Task<IActionResult> ViewAttemptsResult(int attemptid)
        {
            int studentId = _currentUserService.UserId;
            var result = await _mediator.Send(new ViewAttemptResults(studentId, attemptid));
            return Ok(result);
        }

    }
}
