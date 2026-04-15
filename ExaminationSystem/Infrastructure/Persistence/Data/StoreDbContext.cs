using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Persistence.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
        public DbSet<Attempts> Attempts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Diploma> Diplomas { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }


    }
}
