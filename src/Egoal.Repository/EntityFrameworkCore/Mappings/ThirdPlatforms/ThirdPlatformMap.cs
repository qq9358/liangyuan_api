using Egoal.ThirdPlatforms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.ThirdPlatforms
{
    public class ThirdPlatformMap : IEntityTypeConfiguration<ThirdPlatform>
    {
        public void Configure(EntityTypeBuilder<ThirdPlatform> entity)
        {
            entity.ToTable("MM_ThirdPartyPlatform");

            entity.HasIndex(e => e.Uid)
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .HasMaxLength(20)
                .IsUnicode(false)
                .ValueGeneratedNever();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.OrderCheckUrl)
                .HasMaxLength(200);

            entity.Property(e => e.Pwd)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Uid)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
