using ExaminationSystem.BuildingBlocks.Helpers;
using ExaminationSystem.Features.Attempts.GetAttemptDetailsForAdmin;
using ExaminationSystem.Features.Attempts.studemtAttemptsForAdmin;
using ExaminationSystem.Features.Diplomas.CreateDiploma;
using ExaminationSystem.Features.Diplomas.DeleteDiploma;
using ExaminationSystem.Features.Diplomas.UpdateDiploma;
using ExaminationSystem.Features.Quizzes.CreateQuiz;
using ExaminationSystem.Features.Quizzes.CreateQuiz.CreateQuestions;
using ExaminationSystem.Features.Quizzes.DeleteQuiz;
using ExaminationSystem.Features.Quizzes.DeleteQuiz.DeleteQuestion;
using ExaminationSystem.Features.Quizzes.DTOS;
using ExaminationSystem.Features.Quizzes.UpdateQuiz;
using ExaminationSystem.Features.Quizzes.UpdateQuiz.UpdateQuestions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("diplomas")]
        public async Task<IActionResult> CreateDiploma(string title, string? description)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new CreateDiplomaCommand(title, description)),
                result => StatusCode(StatusCodes.Status201Created, result));
        }

        [HttpPut("diplomas/{id:int}")]
        public async Task<IActionResult> UpdateDiploma(int id, string title, string? description)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new UpdateDiplomaCommand(id, title, description)),
                result => Ok(result));
        }

        [HttpDelete("diplomas/{id:int}")]
        public async Task<IActionResult> DeleteDiploma(int id)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new DeleteDiplomaCommand(id)),
                result => Ok(result));
        }

        [HttpPost("quizzes")]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDto dto)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new CreateQuizCommand(dto)),
                result => StatusCode(StatusCodes.Status201Created, result));
        }

        [HttpPut("quizzes/{quizId:int}")]
        public async Task<IActionResult> UpdateQuiz(int quizId, [FromBody] UpdateQuizDto dto)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new UpdateQuizCommand(quizId, dto)),
                result => Ok(result));
        }

        [HttpDelete("quizzes/{quizId:int}")]
        public async Task<IActionResult> DeleteQuiz(int quizId)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new DeleteQuizCommand(quizId)),
                result => Ok(result));
        }

        [HttpPost("quizzes/{quizId:int}/questions")]
        public async Task<IActionResult> CreateQuestion(int quizId, [FromBody] CreateQuestionsforQuizDto dto)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new CreateQuestionsCommand(quizId, dto)),
                result => StatusCode(StatusCodes.Status201Created, result));
        }

        [HttpPut("questions/{questionId:int}")]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] CreateQuestionsforQuizDto dto)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new UpdateQuestionsCommand(questionId, dto)),
                result => Ok(result));
        }

        [HttpDelete("questions/{questionId:int}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new DeleteQuestionCommand(questionId)),
                result => Ok(result));
        }

        [HttpGet("attempts")]
        public async Task<IActionResult> GetAllAttemptsForAdmin([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new ViewStudentAttemptsForAdmin(pageIndex, pageSize));
            return Ok(result);
        }


        [HttpGet("attempts/{id}")]
        public async Task<IActionResult> GetAttemptByIdForAdmin(int id)
        {
            var result = await _mediator.Send(new GetDetailsAttemptByIdForAdminQuery(id));
            return Ok(result);
        }
    }
}
