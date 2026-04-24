using ExaminationSystem.BuildingBlocks.Interfaces;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Infrastructure.Persistence
{
    public class Dataseeding : IDataseeding
    {
        private readonly ExamAppDbContext _context;
        private readonly UserManager<User> _userManager;

        public Dataseeding(ExamAppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task SeedDataAsync()
        {
            await SeedUsersAsync();
            //await SeedDiplomasAsync();
            //await SeedQuizzesAsync();
            //await SeedQuestionsAsync();
            //await SeedOptionsAsync();
            //await SeedEnrollmentsAsync();
            //await SeedAttemptsAsync();
            //await SeedStudentAnswersAsync();
        }

        
        

        private async Task SeedUsersAsync()
        {
            if (_userManager.Users.Any()) return;

            var users = new[]
{
                new { Email = "admin@exam.com", UserName = "admin@exam.com", Password = "Admin@1234!" },
                new { Email = "alice@exam.com", UserName = "alice@exam.com", Password = "Student@1234!" },
                new { Email = "bob@exam.com", UserName = "bob@exam.com", Password = "Student@1234!" },
                new { Email = "carol@exam.com", UserName = "carol@exam.com", Password = "Student@1234!" },
            };

            foreach (var u in users)
            {
                var user = new User
                {
                    Email = u.Email,
                    UserName = u.UserName,
                    EmailConfirmed = true,
                    FullName = u.UserName,
                    Status = UserStatus.Active
                };

                await _userManager.CreateAsync(user, u.Password);
            }
        }
        
       



    }
}
