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

            builder.Entity<User>()
                  .HasDiscriminator<string>("UserType")
                  .HasValue<User>("User")
                  .HasValue<Admin>("Admin")
                  .HasValue<Student>("Student");
        }

        public override int SaveChanges()
        {
            ApplyCreatedAtRules();
            ApplySoftDeleteRules();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyCreatedAtRules();
            ApplySoftDeleteRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyCreatedAtRules();
            ApplySoftDeleteRules();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyCreatedAtRules();
            ApplySoftDeleteRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyCreatedAtRules()
        {
            foreach (var entry in ChangeTracker.Entries<IBaseEntity>().Where(e => e.State == EntityState.Added))
            {
                if (entry.Entity.CreatedAt == default)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
            }
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
    }
}
