using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Features.Users.DTOs;
using ExaminationSystem.Features.Users.Login;
using ExaminationSystem.Features.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator; ////////

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Register a new student account.
        /// On success the account is created in 'Pending' status and a 6-digit OTP
        /// is sent to the provided email address for verification.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<RegisterResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<RegisterResponseDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<RegisterResponseDto>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegisterationDto user)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<RegisterResponseDto>.Fail(
                    "Validation failed.", validationErrors));
            }

            var result = await _mediator.Send(new RegisterUserCommand(user));

            if (!result.isSuccess)
            {
                // Duplicate email → 409 Conflict
                var isConflict = result.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase);
                return isConflict
                    ? Conflict(result)
                    : BadRequest(result);
            }

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto,CancellationToken cancellationToken)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var result = await _mediator.Send(new LoginUserCommand(dto, ipAddress), cancellationToken);

            if (!result.isSuccess)
            {
                return result.StatusCode switch
                {
                    401 => Unauthorized(result),
                    403 => StatusCode(StatusCodes.Status403Forbidden, result),
                    429 => StatusCode(StatusCodes.Status429TooManyRequests, result),
                    _ => BadRequest(result),
                };
            }

            // Write refresh token into HttpOnly cookie — never exposed in response body
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

            return Ok(result);

        }
    }
}
