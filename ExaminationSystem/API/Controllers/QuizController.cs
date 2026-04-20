using ExaminationSystem.Features.SubmitQuiz.Dtos;
using ExaminationSystem.Features.SubmitQuiz.Orchestrators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [ApiController]
    [Route("api/quizzes")]
    public class QuizController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuizController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{quizId}/submit")]
        public async Task<IActionResult> SubmitQuiz(int quizId, [FromBody] ICollection<QuizAnswerDto> quizAnswers)
        {
            var userId = int.Parse(User.FindFirst("userId")!.Value);

            var result = await _mediator.Send(new SubmitQuizOrchestrator(
                userId,
                quizId,
                quizAnswers));

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
    }
}