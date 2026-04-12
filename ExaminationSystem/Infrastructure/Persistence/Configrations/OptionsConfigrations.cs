using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configrations
{
    public class OptionsConfigrations : IEntityTypeConfiguration<Options>
    {
        public void Configure(EntityTypeBuilder<Options> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasQueryFilter(o =>
                !o.Question.IsDeleted &&
                !o.Question.Quiz.IsDeleted &&
                !o.Question.Quiz.Diploma.IsDeleted);

            builder.HasOne(o => o.Question)
               .WithMany(q => q.Options)
               .HasForeignKey(o => o.QuestionId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.StudentAnswers)
                .WithOne(answer => answer.SelectedOption)
                .HasForeignKey(answer => answer.SelectedOptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
