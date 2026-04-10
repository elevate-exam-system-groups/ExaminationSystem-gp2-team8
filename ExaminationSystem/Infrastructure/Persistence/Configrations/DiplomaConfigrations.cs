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
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(d => d.status)
            .IsRequired()
            .HasConversion<string>();


            builder.Property(d => d.IsDeleted)
          .HasDefaultValue(false);

            // Relationship
            builder.HasOne(d => d.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(d => d.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
