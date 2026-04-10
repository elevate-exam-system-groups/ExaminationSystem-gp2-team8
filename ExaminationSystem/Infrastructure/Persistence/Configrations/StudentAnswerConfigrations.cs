using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configrations
{
    public class StudentAnswerConfigrations : IEntityTypeConfiguration<StudentAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentAnswer> builder)
        {
            builder.HasOne(x => x.Attempt)
             .WithMany()
             .HasForeignKey(x => x.AttemptId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Question)
                   .WithMany()
                   .HasForeignKey(x => x.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SelectedOption)
                   .WithMany()
                   .HasForeignKey(x => x.SelectedOptionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
