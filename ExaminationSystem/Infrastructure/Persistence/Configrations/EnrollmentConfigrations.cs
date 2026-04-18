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

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(e => e.DeletedAt)
                .IsRequired(false);

            builder.HasQueryFilter(e => !e.IsDeleted && !e.Diploma.IsDeleted);

            // Relationships
            builder.HasOne(e => e.User)
                   .WithMany(u => u.Enrollments)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Diploma)
                   .WithMany(d => d.Enrollments)
                   .HasForeignKey(e => e.DiplomaId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicate enrollment 🚨
            builder.HasIndex(e => new { e.UserId, e.DiplomaId })
                   .IsUnique();

            // Default value
            builder.Property(e => e.EnrollmentDate)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
