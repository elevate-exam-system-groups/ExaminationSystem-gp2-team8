using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Features.Diplomas.Queries;
using ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [Route("api/student/diplomas")]
    [ApiController]
    public class DiplomasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DiplomasController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiplomas([FromQuery] int page = 1, [FromQuery] int perPage = 10, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllDiplomasQuery(page, perPage), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{diplomaId}/quizzes")]
        public async Task<IActionResult> GetDiplomaById(int diplomaId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPublishDiplomaByIdQuery(diplomaId), cancellationToken);

            if (!result.isSuccess)
            {
                return result.Error?.Code switch
                {
                    "NOT_FOUND" => NotFound(result),
                    "FORBIDDEN" => StatusCode(403, result),
                    _ => BadRequest(result)
                };
            }
            return Ok(result);
        }

        

    }
}
