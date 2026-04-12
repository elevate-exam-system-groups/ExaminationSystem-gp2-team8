using ExaminationSystem.Features.Diplomas.CreateDiploma;
using ExaminationSystem.Features.Diplomas.DeleteDiploma;
using ExaminationSystem.Features.Diplomas.DTOS;
using ExaminationSystem.Features.Diplomas.UpdateDiploma;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [Route("api/admin/diplomas")]
    [ApiController]
    public class AdminDiplomasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminDiplomasController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateDiploma(string title, string? description)
        {
            var result = await _mediator.Send(new CreateDiplomaCommand(title, description));
            if (!result.isSuccess) return StatusCode(400, result.Error);
            return StatusCode(201, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiploma(int id, string title, string? description)
        {
            var result = await _mediator.Send(new UpdateDiplomaCommand(id, title, description));
            if (!result.isSuccess) return StatusCode(400, result.Error);
            return StatusCode(201, result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiploma(int id)
        {
            var result = await _mediator.Send(new DeleteDiplomaCommand(id));
            if (!result.isSuccess) return StatusCode(400, result.Error);
            return StatusCode(201, result.Data);
        }
    }
}
