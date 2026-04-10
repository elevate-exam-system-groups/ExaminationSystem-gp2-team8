using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configrations
{
    public class OptionsConfigrations : IEntityTypeConfiguration<Options>
    {
        public void Configure(EntityTypeBuilder<Options> builder)
        {
            builder.HasOne(o => o.Question)
               .WithMany(q => q.Options)
               .HasForeignKey(o => o.QuestionId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
