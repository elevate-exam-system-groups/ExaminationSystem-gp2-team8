using ExaminationSystem.Features.Quizzes.CreateQuiz;
using ExaminationSystem.Features.Quizzes.CreateQuiz.CreateQuestions;
using ExaminationSystem.Features.Quizzes.DeleteQuiz;
using ExaminationSystem.Features.Quizzes.DeleteQuiz.DeleteQuestion;
using ExaminationSystem.Features.Quizzes.DTOS;
using ExaminationSystem.Features.Quizzes.UpdateQuiz;
using ExaminationSystem.Features.Quizzes.UpdateQuiz.UpdateQuestions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminQuizzesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminQuizzesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("quizzes")]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDto dto)
        {
            var result = await _mediator.Send(new CreateQuizCommand(dto));

            if (!result.isSuccess) return MapFailure(result);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPut("quizzes/{quizId:int}")]
        public async Task<IActionResult> UpdateQuiz(int quizId, [FromBody] UpdateQuizDto dto)
        {
            var result = await _mediator.Send(new UpdateQuizCommand(quizId, dto));

            if (!result.isSuccess) return MapFailure(result);

            return Ok(result);
        }

        [HttpDelete("quizzes/{quizId:int}")]
        public async Task<IActionResult> DeleteQuiz(int quizId)
        {
            var result = await _mediator.Send(new DeleteQuizCommand(quizId));

            if (!result.isSuccess) return MapFailure(result);

            return Ok(result);
        }

        [HttpPost("quizzes/{quizId:int}/questions")]
        public async Task<IActionResult> CreateQuestion(int quizId, [FromBody] CreateQuestionsforQuizDto dto)
        {
            var result = await _mediator.Send(new CreateQuestionsCommand(quizId, dto));

            if (!result.isSuccess) return MapFailure(result);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPut("questions/{questionId:int}")]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] CreateQuestionsforQuizDto dto)
        {
            var result = await _mediator.Send(new UpdateQuestionsCommand(questionId, dto));

            if (!result.isSuccess) return MapFailure(result);

            return Ok(result);
        }

        [HttpDelete("questions/{questionId:int}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            var result = await _mediator.Send(new DeleteQuestionCommand(questionId));

            if (!result.isSuccess) return MapFailure(result);

            return Ok(result);
        }

        private IActionResult MapFailure<T>(BuildingBlocks.ExceptionHandling.ApiResponse<T> result)
        {
            return result.Error?.Code switch
            {
                "404" => NotFound(result),
                "409" => Conflict(result),
                "422" => UnprocessableEntity(result),
                _ => BadRequest(result)
            };
        }
    }
}
