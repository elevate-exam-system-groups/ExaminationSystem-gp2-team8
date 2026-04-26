using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.Domain.Common;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Users.DTOs;
using ExaminationSystem.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Users.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<LoginResponseDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginUserHandler> _logger;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginUserHandler(UserManager<User> userManager,
             SignInManager<User> signInManager,
             ILogger<LoginUserHandler> logger,
             IJwtService jwtService,
             IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<Result<LoginResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // check for email
            var user = await _userManager.FindByEmailAsync(dto.Email);

            //Unknown email — return generic 401 (don't reveal which field is wrong)
            if (user is null)
            {
                
                return Result<LoginResponseDto>.Failure("Invalid credentials.", statusCode: 401);
            }

            //Account locked 
            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogWarning("Locked account login attempt: {Email} from {Ip}", dto.Email, request.IpAddress);
                
                return Result<LoginResponseDto>.Failure("Account is temporarily locked due to too many failed attempts. Try again in 15 minutes.", statusCode: 429);
            }
            //Account not verified (status = Pending)
            if (user.Status == UserStatus.Pending)
            {
                
                return Result<LoginResponseDto>.Failure("Account not verified. Please check your email for the OTP verification code.", statusCode: 403);
            }

            //Soft-deleted account
            if (user.DeletedAt is not null)
            {
                return Result<LoginResponseDto>.Failure("Invalid credentials.", statusCode: 401);
            }

            //Validate password (SignInManager respects lockout settings)
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
            if (!signInResult.Succeeded)
            {
                // Check if this failed attempt just triggered a lockout
                if (signInResult.IsLockedOut)
                {
                    _logger.LogWarning("Account locked after failed attempts: {UserId}", user.Id);
                    return Result<LoginResponseDto>.Failure("Too many failed attempts. Account locked for 15 minutes.", statusCode: 429);
                }
                var remaining = _userManager.Options.Lockout.MaxFailedAccessAttempts - await _userManager.GetAccessFailedCountAsync(user);

                return Result<LoginResponseDto>.Failure($"Invalid credentials. {remaining} attempt(s) remaining before lockout.", statusCode: 401);
            }

            //Reset failed login counter on success
            await _userManager.ResetAccessFailedCountAsync(user);

            // Get user roles for JWT payload
            var roles = await _userManager.GetRolesAsync(user);

            // Generate JWT access token (15 min)
            var (accessToken, expiresAt) = _jwtService.GenerateAccessToken(user, roles);

            // Generate refresh token (7 days) — caller writes HttpOnly cookie
            var rawRefreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user.Id, request.IpAddress, cancellationToken);

            // Log successful attempt 
            
            _logger.LogInformation("User {UserId} logged in from {Ip}", user.Id, request.IpAddress);

            var response = new LoginResponseDto
            {
                AccessToken = accessToken,
                AccessTokenExpiresAt = expiresAt,
                TokenType = "Bearer",
                // RawRefreshToken is returned separately so the controller
                // can write it into an HttpOnly cookie
                User = new UserInfoDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    Role = roles.FirstOrDefault() ?? "Student",
                }
            };
            if (response is null)
                return Result<LoginResponseDto>.Failure("No data found.", 404);
            return Result<LoginResponseDto>.Success(response) with { RefreshToken = rawRefreshToken };
            // Attach raw refresh token in a transient property so the controller can cookie it
            // ApiResponse<LoginResponseDto>.Ok(response, "Login successful.") with { RefreshToken = rawRefreshToken };
        }
    }
}
