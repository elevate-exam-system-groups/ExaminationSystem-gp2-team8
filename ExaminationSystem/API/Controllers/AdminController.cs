using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Helpers;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Features.AdminStats;
using ExaminationSystem.Features.AdminStats.DTOs;
using ExaminationSystem.Features.Analytics.DTO;
using ExaminationSystem.Features.Analytics.Orchestrators;
using ExaminationSystem.Features.Attempts.DTOs;
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
using ExaminationSystem.Features.Quizzes.PublishQuiz;
using ExaminationSystem.Features.Quizzes.UnpublishQuiz;
using ExaminationSystem.Features.Quizzes.UpdateQuiz;
using ExaminationSystem.Features.Quizzes.UpdateQuiz.UpdateQuestions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    // [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ── Diplomas ────────────────────────────────────────────────────

        [HttpPost("diplomas")]
        public async Task<IActionResult> CreateDiploma(string title, string? description)
        {
            var result = await _mediator.Send(new CreateDiplomaCommand(title, description));
            return StatusCode(StatusCodes.Status201Created, ApiResponse<object>.Ok(result, "Diploma created."));
        }

        [HttpPut("diplomas/{id:int}")]
        public async Task<IActionResult> UpdateDiploma(int id, string title, string? description)
        {
            var result = await _mediator.Send(new UpdateDiplomaCommand(id, title, description));
            return Ok(ApiResponse<object>.Ok(result, "Diploma updated."));
        }

        [HttpDelete("diplomas/{id:int}")]
        public async Task<IActionResult> DeleteDiploma(int id)
        {
            var result = await _mediator.Send(new DeleteDiplomaCommand(id));
            return Ok(ApiResponse<object>.Ok(result, "Diploma deleted."));
        }

     

        [HttpPost("quizzes")]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDto dto)
        {
            var result = await _mediator.Send(new CreateQuizCommand(dto));
            return StatusCode(StatusCodes.Status201Created, ApiResponse<object>.Ok(result, "Quiz created."));
        }

        [HttpPut("quizzes/{quizId:int}")]
        public async Task<IActionResult> UpdateQuiz(int quizId, [FromBody] UpdateQuizDto dto)
        {
            var result = await _mediator.Send(new UpdateQuizCommand(quizId, dto));
            return Ok(ApiResponse<object>.Ok(result, "Quiz updated."));
        }

        [HttpDelete("quizzes/{quizId:int}")]
        public async Task<IActionResult> DeleteQuiz(int quizId)
        {
            var result = await _mediator.Send(new DeleteQuizCommand(quizId));
            return Ok(ApiResponse<object>.Ok(result, "Quiz deleted."));
        }

        [HttpPatch("quizzes/{quizId:int}/publish")]
        public async Task<IActionResult> PublishQuiz(int quizId)
        {
            var result = await _mediator.Send(new PublishQuizCommand(quizId));
            return Ok(ApiResponse<object>.Ok(result, "Quiz published."));
        }

        [HttpPatch("quizzes/{quizId:int}/unpublish")]
        public async Task<IActionResult> UnpublishQuiz(int quizId)
        {
            var result = await _mediator.Send(new UnpublishQuizCommand(quizId));
            return Ok(ApiResponse<object>.Ok(result, "Quiz unpublished."));
        }



        [HttpPost("quizzes/{quizId:int}/questions")]
        public async Task<IActionResult> CreateQuestion(int quizId, [FromBody] CreateQuestionsforQuizDto dto)
        {
            var result = await _mediator.Send(new CreateQuestionsCommand(quizId, dto));
            return StatusCode(StatusCodes.Status201Created, ApiResponse<object>.Ok(result, "Question created."));
        }

        [HttpPut("questions/{questionId:int}")]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] CreateQuestionsforQuizDto dto)
        {
            var result = await _mediator.Send(new UpdateQuestionsCommand(questionId, dto));
            return Ok(ApiResponse<object>.Ok(result, "Question updated."));
        }

        [HttpDelete("questions/{questionId:int}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            var result = await _mediator.Send(new DeleteQuestionCommand(questionId));
            return Ok(ApiResponse<object>.Ok(result, "Question deleted."));
        }

     

        [HttpGet("attempts")]
        public async Task<IActionResult> GetAllAttemptsForAdmin([FromQuery] PaginationParams pagination)
        {
            var result = await _mediator.Send(new ViewStudentAttemptsForAdmin(pagination.Page, pagination.PerPage));
            return Ok(ApiResponse<object>.Ok(result));
        }

        [HttpGet("attempts/{id:int}")]
        public async Task<IActionResult> GetAttemptByIdForAdmin(int id)
        {
            var result = await _mediator.Send(new GetAttemptAdminDetailsByIdOrchestrator(id));
            return Ok(ApiResponse<object>.Ok(result));
        }

        [HttpGet("attempts/{quizId:int}/{studentId:int}")]
        public async Task<IActionResult> GetAttemptsByQuizIdAndStudentId(
            int quizId,
            int studentId,
            [FromQuery] PaginationParams pagination)
        {
            var result = await _mediator.Send(new GetStudentByQuizIdandStudntIdQuery(pagination, quizId, studentId));
            return Ok(ApiResponse<PaginatedResult<FilteredAttemptsDTO>>.Ok(result));
        }

        // ── Stats & Analytics ───────────────────────────────────────────

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAdminStatsQuery(), cancellationToken);

            if (result.IsFailure)
                return StatusCode(result.StatusCode,
                    ApiResponse<AdminStatsDto>.Fail(result.Error));

            return Ok(ApiResponse<AdminStatsDto>.Ok(result.Value!, "Stats retrieved successfully."));
        }

        [HttpGet("analytics")]
        public async Task<IActionResult> GetAnalytics([FromQuery] FiltersElement filters)
        {
            var result = await _mediator.Send(new AnalyticsOrchestrator(filters));
            return Ok(ApiResponse<AnalysticsWithNoFilterDTO>.Ok(result));
        }
    }
}