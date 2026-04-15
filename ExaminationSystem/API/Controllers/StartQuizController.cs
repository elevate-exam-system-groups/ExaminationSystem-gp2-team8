using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.StartQuiz.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [ApiController]
    [Route("api/quizzes")]
    public class StartQuizController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StartQuizController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{quizId}/start")]
        public async Task<ActionResult<Result<QuizAttemptDto>>> StartQuiz(int quizId,[FromQuery] int userId)
        {
            var result = await _mediator.Send(new StartQuizOrchestrator(quizId, userId));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}