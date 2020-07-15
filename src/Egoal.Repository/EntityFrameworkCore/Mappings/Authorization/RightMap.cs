using Egoal.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Authorization
{
    public class RightMap : IEntityTypeConfiguration<Right>
    {
        public void Configure(EntityTypeBuilder<Right> entity)
        {
            entity.HasKey(e => e.UniqueCode);

            entity.HasIndex(e => e.Id);

            entity.HasIndex(e => e.Pid);

            entity.Property(e => e.UniqueCode)
                .ValueGeneratedNever();

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.MenuTypeId)
                .HasColumnName("MenuTypeID");

            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.Property(e => e.Pid)
                .HasColumnName("PID");

            entity.Property(e => e.SystemTypeId)
                .HasColumnName("SystemTypeID");

            entity.Property(e => e.Value)
                .HasMaxLength(100);

            entity.Property(e => e.Version)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.VisibleTypeId)
                .HasColumnName("VisibleTypeID");

            entity.ToTable("RM_Right");
        }
    }
}
