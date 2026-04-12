using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configrations
{
    public class QuestionConfigrations : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(question => question.Id);

            builder.Property(q => q.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(q => q.DeletedAt)
                .IsRequired(false);

            builder.HasQueryFilter(q =>
                !q.IsDeleted &&
                !q.Quiz.IsDeleted &&
                !q.Quiz.Diploma.IsDeleted);

            builder.HasOne(question => question.Quiz)
                .WithMany(quiz => quiz.Questions)
                .HasForeignKey(question => question.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(question => question.CreatedByUser)
                .WithMany(user => user.CreatedQuestions)
                .HasForeignKey(question => question.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(question => question.Options)
                .WithOne(option => option.Question)
                .HasForeignKey(option => option.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(question => question.StudentAnswers)
                .WithOne(answer => answer.Question)
                .HasForeignKey(answer => answer.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
