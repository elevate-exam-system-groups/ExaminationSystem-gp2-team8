using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configrations
{
    public class QuizConfigrations : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(q => q.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(q => q.DeletedAt)
                .IsRequired(false);

            builder.HasQueryFilter(q => !q.IsDeleted && !q.Diploma.IsDeleted);

            builder.HasOne(q => q.CreatedByUser)
                .WithMany(u => u.CreatedQuizzes)
                .HasForeignKey(q => q.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.Diploma)
                .WithMany(d => d.Quizzes)
                .HasForeignKey(q => q.DiplomaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(q => q.Questions)
                .WithOne(question => question.Quiz)
                .HasForeignKey(question => question.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(q => q.Attempts)
                .WithOne(attempt => attempt.Quiz)
                .HasForeignKey(attempt => attempt.QuizId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
