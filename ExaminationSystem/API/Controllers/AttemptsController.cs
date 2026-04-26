using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Features.AnswerQuestion;
using ExaminationSystem.Features.AnswerQuestion.DTOs;
using ExaminationSystem.Features.Attempts;
using ExaminationSystem.Features.Attempts.GetAttemptDetails;
using ExaminationSystem.Features.Attempts.GetAttempts;
using ExaminationSystem.Features.Attempts.GetAttemptResult;
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
            [FromQuery] int? quizId,
            [FromQuery] int? diplomaId,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 20)
        {
            int studentId = _currentUserService.UserId != 0 ? _currentUserService.UserId : 3;
            var result = await _mediator.Send(new GetStudentAttemptQuery(studentId, quizId, diplomaId, page, perPage));
            return Ok(ApiResponse<object>.Ok(result));
        }

        [HttpGet("{attemptId:int}")]
        public async Task<IActionResult> GetAttemptDetails(int attemptId)
        {
            int studentId = _currentUserService.UserId != 0 ? _currentUserService.UserId : 1;
            var result = await _mediator.Send(new GetStudentAttemptDetailsQuery(studentId, attemptId));
            return Ok(ApiResponse<object>.Ok(result));
        }

        [HttpGet("{attemptId:int}/results")]
        public async Task<IActionResult> ViewAttemptsResult(int attemptId)
        {
            int studentId = 3;
            var result = await _mediator.Send(new ViewAttemptResults(studentId, attemptId));
            return Ok(ApiResponse<object>.Ok(result));
        }

        [HttpPost("{attemptId:int}/answer")]
        public async Task<IActionResult> SubmitAnswer(
            [FromRoute] int attemptId,
            [FromBody] SubmitAnswerRequestDto dto,
            CancellationToken cancellationToken)
        {
            int studentId = _currentUserService.UserId != 0 ? _currentUserService.UserId : 1;
            var result = await _mediator.Send(new SubmitAnswerCommand(attemptId, studentId, dto), cancellationToken);
            return Ok(ApiResponse<object>.Ok(result));
        }
    }
}