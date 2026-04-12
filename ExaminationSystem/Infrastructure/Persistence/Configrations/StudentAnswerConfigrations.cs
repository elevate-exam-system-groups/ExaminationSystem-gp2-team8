using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configrations
{
    public class StudentAnswerConfigrations : IEntityTypeConfiguration<StudentAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentAnswer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x =>
                !x.Question.IsDeleted &&
                !x.Question.Quiz.IsDeleted &&
                !x.Question.Quiz.Diploma.IsDeleted);

            builder.HasOne(x => x.User)
                .WithMany(u => u.StudentAnswers)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Attempt)
             .WithMany(a => a.StudentAnswers)
             .HasForeignKey(x => x.AttemptId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Question)
                   .WithMany(q => q.StudentAnswers)
                   .HasForeignKey(x => x.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SelectedOption)
                   .WithMany(o => o.StudentAnswers)
                   .HasForeignKey(x => x.SelectedOptionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
