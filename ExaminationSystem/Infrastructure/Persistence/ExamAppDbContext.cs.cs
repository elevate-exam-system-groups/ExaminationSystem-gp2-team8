using ExaminationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Persistence
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

        public override int SaveChanges()
        {
            ApplySoftDeleteRules();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplySoftDeleteRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplySoftDeleteRules();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplySoftDeleteRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplySoftDeleteRules()
        {
            foreach (var entry in ChangeTracker.Entries<Diploma>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt ??= DateTime.UtcNow;
            }

            foreach (var entry in ChangeTracker.Entries<Quiz>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt ??= DateTime.UtcNow;
            }

            foreach (var entry in ChangeTracker.Entries<Question>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt ??= DateTime.UtcNow;
            }
        }

        public DbSet<Diploma> Diplomas { get; set; }
        public DbSet<Attempts> Attempts { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
        //public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
