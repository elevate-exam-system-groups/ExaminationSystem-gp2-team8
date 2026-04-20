using ExaminationSystem.Features.Diplomas.Queries;
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


    }
}
