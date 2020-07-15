using Egoal.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Common
{
    public class AreaMap : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.KeYuanTypeId)
                .HasColumnName("KeYuanTypeID");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Pid)
                .HasColumnName("PID");

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);

            entity.HasIndex(e => e.Pid);

            entity.ToTable("SM_Area");
        }
    }
}
