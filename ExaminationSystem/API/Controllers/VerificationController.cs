using ExaminationSystem.Domain.Common;
using ExaminationSystem.Features.Verification.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [ApiController]
    [Route("api/verification")]
    public class VerificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VerificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("send-code")]
        public async Task<IActionResult> SendCode(SendVerificationCodeCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(VerifyAccountCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}