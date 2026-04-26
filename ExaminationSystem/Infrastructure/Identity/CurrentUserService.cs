using ExaminationSystem.BuildingBlocks.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ExaminationSystem.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId => GetUserId();

        private int GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = user?.FindFirst("id")?.Value
                ?? user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? user?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }
}