using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Pagination;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Features.Diplomas.Queries;
using ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas;

using ExaminationSystem.Features.Diplomas.Queries.GetPublishedDiplomaById;

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
        public async Task<IActionResult> GetAllDiplomas([FromQuery] PaginationParams Params, CancellationToken cancellationToken)
        {
            int userId = 3;
            var result = await _mediator.Send(new GetAllpublishedDiplomasQuery(Params, userId), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{diplomaId}/quizzes")]
        public async Task<IActionResult> GetDiplomaById(int diplomaId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPublishDiplomaByIdQuery(diplomaId), cancellationToken);
            return Ok(result);
        }

        

    }
}
