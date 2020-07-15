using Egoal.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Settings
{
    public class TimePointMap : IEntityTypeConfiguration<TimePoint>
    {
        public void Configure(EntityTypeBuilder<TimePoint> entity)
        {
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);

            entity.ToTable("OM_TimePoint");
        }
    }
}
