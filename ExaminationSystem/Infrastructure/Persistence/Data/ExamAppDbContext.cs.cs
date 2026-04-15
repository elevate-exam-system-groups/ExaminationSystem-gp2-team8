using ExaminationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Persistence.Data
{
    public class ExamAppDbContext
        : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ExamAppDbContext(DbContextOptions<ExamAppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ExamAppDbContext).Assembly);
        }

        public DbSet<Diploma> Diplomas { get; set; }
        public DbSet<Attempts> Attempts { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
    }
}