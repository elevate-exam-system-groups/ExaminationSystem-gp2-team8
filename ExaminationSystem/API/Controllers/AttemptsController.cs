using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Features.AnswerQuestion;
using ExaminationSystem.Features.AnswerQuestion.DTOs;
using ExaminationSystem.Features.Attempts.GetAttemptDetails;
using ExaminationSystem.Features.Attempts.GetAttempts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var query = new GetStudentAttemptQuery(studentId, quizId, diplomaId, page, perPage);
            var history = await _mediator.Send(query);

            if (!history.Data.Any()) return NotFound();
            return Ok(history);
        }

        [HttpGet("{attemptId:int}")]
        public async Task<IActionResult> GetAttemptDetails(int attemptId)
        {
            int studentId = _currentUserService.UserId != 0 ? _currentUserService.UserId : 1;
            var result = await _mediator.Send(new GetStudentAttemptDetailsQuery(studentId, attemptId));
            return Ok(result);
        }

        [HttpPost("{attemptId:int}/answer")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitAnswer([FromRoute] int attemptId,
            [FromBody] SubmitAnswerRequestDto dto,
            CancellationToken cancellationToken)
        {
          
            //Extract user ID from JWT claims 
            //var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            //               ?? User.FindFirstValue("sub");

            //if (!int.TryParse(userIdClaim, out var currentUserId))
            //    return Unauthorized(ApiResponse<SubmitAnswerResponseDto>.Fail("Invalid token.", statusCode: 401));

            int studentId = _currentUserService.UserId != 0 ? _currentUserService.UserId : 1;

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
    }
}
