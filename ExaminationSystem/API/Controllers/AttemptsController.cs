using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Features.AnswerQuestion;
using ExaminationSystem.Features.AnswerQuestion.DTOs;
using ExaminationSystem.BuildingBlocks.Helpers;
using ExaminationSystem.Features.Attempts.GetAttemptDetails;
using ExaminationSystem.Features.Attempts.GetAttempts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExaminationSystem.Features.Attempts.GetAttemptResult;

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
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetQuizHistory(
            [FromQuery] int? quizId, 
            [FromQuery] int? diplomaId, 
            [FromQuery] int page= 1, 
            [FromQuery] int perPage= 20)
        {
            if (!TryGetAuthenticatedUserId(out var studentId))
                return Unauthorized(ApiResponse<object>.FailureResponse("Invalid token.", "401"));

            return await ControllerHelper.ExecuteAsync(
                async () =>
                {
                    var query = new GetStudentAttemptQuery(studentId, quizId, diplomaId, page, perPage);
                    return await _mediator.Send(query);
                },
                history => history.Data.Count == 0 ? NotFound() : Ok(history));
        }

        [HttpGet("{attemptId:int}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetAttemptDetails(int attemptId)
        {
            if (!TryGetAuthenticatedUserId(out var studentId))
                return Unauthorized(ApiResponse<object>.FailureResponse("Invalid token.", "401"));

            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new GetStudentAttemptDetailsQuery(studentId, attemptId)),
                Ok);
        }


        [HttpGet("{attemptid}/results")]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> ViewAttemptsResult(int attemptid)
        {
            if (!TryGetAuthenticatedUserId(out var userId))
                return Unauthorized(ApiResponse<object>.FailureResponse("Invalid token.", "401"));

            var result = await _mediator.Send(new ViewAttemptResults(userId, attemptid));
            return Ok(result);
        }

        [HttpPost("{attemptId:int}/answer")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitAnswer([FromRoute] int attemptId,
            [FromBody] SubmitAnswerRequestDto dto,
            CancellationToken cancellationToken)
        {
            if (!TryGetAuthenticatedUserId(out var studentId))
                return Unauthorized(ApiResponse<SubmitAnswerResponseDto>.Fail("Invalid token.", statusCode: 401));

            var result = await _mediator.Send(
                new SubmitAnswerCommand(attemptId, studentId, dto),
                cancellationToken);

            return result.StatusCode switch
            {
                200 => Ok(result),
                403 => StatusCode(StatusCodes.Status403Forbidden, result),
                404 => NotFound(result),
                409 => Conflict(result),
                410 => StatusCode(StatusCodes.Status410Gone, result),
                422 => UnprocessableEntity(result),
                _ => BadRequest(result),
            };
        }

        private bool TryGetAuthenticatedUserId(out int userId)
        {
            userId = _currentUserService.UserId;
            return userId > 0;
        }
    }
}
