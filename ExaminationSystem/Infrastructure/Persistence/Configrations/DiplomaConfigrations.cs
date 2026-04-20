using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configrations
{
    public class DiplomaConfigrations : IEntityTypeConfiguration<Diploma>
    {
        public void Configure(EntityTypeBuilder<Diploma> builder)
        {
            builder.HasKey(d => d.Id);

            // Title
            builder.Property(d => d.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            // Description
            builder.Property(d => d.Description)
                   .HasMaxLength(1000);

            builder.Property(d => d.status)
            .IsRequired()
            .HasConversion<string>();

            builder.Property(d => d.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(d => d.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(d => d.DeletedAt)
                .IsRequired(false);

            builder.HasQueryFilter(d => !d.IsDeleted);

            // Relationship
            builder.HasOne(d => d.CreatedByUser)
                   .WithMany(u => u.CreatedDiplomas)
                   .HasForeignKey(d => d.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Quizzes)
                   .WithOne(q => q.Diploma)
                   .HasForeignKey(q => q.DiplomaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Enrollments)
                   .WithOne(e => e.Diploma)
                   .HasForeignKey(e => e.DiplomaId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
