using ExaminationSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Domain.Entities
{
    public class User:IdentityUser<int>, IBaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string FullName { get; set; } = null!;

        public string? OtpHash { get; set; }

        public DateTime? OtpExpiry { get; set; }

        public string? RefreshToken { get; set; }

        public UserStatus Status { get; set; }
       
      
    

    }
}
