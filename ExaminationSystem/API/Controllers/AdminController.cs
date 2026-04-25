using ExaminationSystem.BuildingBlocks.Helpers;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Features.Analytics.Orchestrators;
using ExaminationSystem.Features.Attempts.DTOs;
using ExaminationSystem.Features.Attempts.GetAttemptDetailsForAdmin;
using ExaminationSystem.Features.Attempts.GetStudentByQuizIdandStudntId;
using ExaminationSystem.Features.Attempts.Orchestrators;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    //[Authorize(Roles = "Admin")]
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
        public async Task<ActionResult<PaginatedResult<AllAttemptForAdminDTO>>> GetAllAttemptsForAdmin([FromQuery] PaginationParams pagination)
        {
            var result = await _mediator.Send(new ViewStudentAttemptsForAdmin(pagination.Page, pagination.PerPage));
            return Ok(result);
        }


        [HttpGet("attempts/{id}")]
        public async Task<ActionResult<AttemptForAdminDetailsDTO>> GetAttemptByIdForAdmin(int id)
        {
            var result = await _mediator.Send(new GetAttemptAdminDetailsByIdOrchestrator(id));
            return Ok(result);
        }

        [HttpGet("attempts/{quizId:int}/{studentId:int}")]
        public async Task<ActionResult<PaginatedResult<FilteredAttemptsDTO>>> GetAttemptsByQuizIdandStudentIdForAdmin(int quizId, int studentId, [FromQuery] PaginationParams pagination)
        {
            var result = await _mediator.Send(new GetStudentByQuizIdandStudntIdQuery(pagination, quizId, studentId));
            return Ok(result);
        }

        [HttpGet("analytics")]
        public async Task<IActionResult> GetAnalyticsWithNoFilters()
        {
            // Implement your analytics logic here, e.g., gather data from the database, perform calculations, etc.
            var analyticsData = await _mediator.Send(new AnalyticswithNoFiltersOrchestrator());

            return Ok(analyticsData);
        }
    }
}
