using ExaminationSystem.BuildingBlocks.Interfaces;

namespace ExaminationSystem.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }



        public int UserId => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value != null
            ? int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")!.Value)
            : 0;
    }
}