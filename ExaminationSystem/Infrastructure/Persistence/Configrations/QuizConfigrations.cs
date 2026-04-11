using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configrations
{
    public class QuizConfigrations : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasOne(q => q.CreatedByUser).WithMany().HasForeignKey(q => q.CreatedByUserId).OnDelete(DeleteBehavior.Restrict); 
            builder.HasOne(q => q.Diploma).WithMany(d=>d.Quizzes).HasForeignKey(q => q.DiplomaId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
