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

        public AttemptsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuizHistory([FromQuery] int quizId, 
            [FromQuery] int diplomaId, 
            [FromQuery] int page= 1, 
            [FromQuery] int perPage= 20)
        {
            int studentId = 1; //Until auth is applied
            var query = new GetStudentAttemptQuery(studentId, quizId, diplomaId, page, perPage);
            var history = await _mediator.Send(query);

            if (!history.Data.Any()) return NotFound();
            return Ok(history);
        }

    }
}
