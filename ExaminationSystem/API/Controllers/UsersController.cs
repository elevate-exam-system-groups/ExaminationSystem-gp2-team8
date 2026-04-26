using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.Users.DTOs;
using ExaminationSystem.Features.Users.Login;
using ExaminationSystem.Features.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegisterationDto user)
        {
            var result = await _mediator.Send(new RegisterUserCommand(user));
            return StatusCode(StatusCodes.Status201Created, ApiResponse<RegisterResponseDto>.Created(result.Value!, "Account created."));
        }

        [HttpPost("login")]
        
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto, CancellationToken cancellationToken)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var result = await _mediator.Send(new LoginUserCommand(dto, ipAddress), cancellationToken);

            if (result.RefreshToken is not null)
            {
                Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                });
            }

            return Ok(ApiResponse<LoginResponseDto>.Ok(result.Value!, "Login successful."));
        }
    }
}