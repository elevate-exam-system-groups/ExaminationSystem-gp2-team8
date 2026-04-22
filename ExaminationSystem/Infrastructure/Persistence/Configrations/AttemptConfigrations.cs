using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configrations
{
    public class AttemptConfigrations : IEntityTypeConfiguration<Attempts>
    {
        public void Configure(EntityTypeBuilder<Attempts> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(a => a.DeletedAt)
                .IsRequired(false);

            builder.HasQueryFilter(a =>
    !a.IsDeleted &&
    !a.Quiz.IsDeleted &&
    !a.Quiz.Diploma.IsDeleted &&
    !a.User.IsDeleted);

            builder.HasOne(a => a.Quiz)
                .WithMany(q => q.Attempts)
                .HasForeignKey(a => a.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.User)
                .WithMany(u => u.Attempts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.StudentAnswers)
                .WithOne(answer => answer.Attempt)
                .HasForeignKey(answer => answer.AttemptId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
