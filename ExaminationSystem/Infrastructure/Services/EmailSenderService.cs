using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ExaminationSystem.Infrastructure.Services
{
    public class EmailSenderService : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"Email to: {email}");
            Console.WriteLine(subject);
            Console.WriteLine(htmlMessage);

            return Task.CompletedTask;
        }
    }
}
