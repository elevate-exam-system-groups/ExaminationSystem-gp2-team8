using ExaminationSystem.BuildingBlocks.Helpers;
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
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new CreateDiplomaCommand(title, description)),
                result => StatusCode(StatusCodes.Status201Created, result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiploma(int id, string title, string? description)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new UpdateDiplomaCommand(id, title, description)),
                result => Ok(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiploma(int id)
        {
            return await ControllerHelper.ExecuteAsync(
                () => _mediator.Send(new DeleteDiplomaCommand(id)),
                result => Ok(result));
        }
    }
}
