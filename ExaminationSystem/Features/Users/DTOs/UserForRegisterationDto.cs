using ExaminationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.Users.DTOs
{
    public class UserForRegisterationDto
    {
       
        [Required(ErrorMessage = "FullName is required")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; init; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }

        
        //public UserStatus UserStatus { get; set; }
    }
}
