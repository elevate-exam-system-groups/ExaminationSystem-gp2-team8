using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configrations
{
    public class EnrollmentConfigrations : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => e.Id);

            // Relationships
            builder.HasOne(e => e.User)
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Diploma)
                   .WithMany()
                   .HasForeignKey(e => e.DiplomaId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Prevent duplicate enrollment 🚨
            builder.HasIndex(e => new { e.UserId, e.DiplomaId })
                   .IsUnique();

            // Default value
            builder.Property(e => e.EnrollmentDate)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
