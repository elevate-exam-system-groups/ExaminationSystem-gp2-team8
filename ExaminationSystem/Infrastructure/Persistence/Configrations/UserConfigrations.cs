using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configrations
{
    public class UserConfigrations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(user => user.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(user => user.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(user => user.Status)
                .HasConversion<string>();

            builder.Property(user => user.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(user => user.DeletedAt)
                .IsRequired(false);

            builder.HasQueryFilter(user => !user.IsDeleted);



            builder.HasDiscriminator<string>("UserType")
            .HasValue<Student>("Student")
            .HasValue<Admin>("Admin");
        }
    }
}
