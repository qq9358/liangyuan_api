using Egoal.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Common
{
    public class ChangCiMap : IEntityTypeConfiguration<ChangCi>
    {
        public void Configure(EntityTypeBuilder<ChangCi> entity)
        {
            entity.ToTable("TM_ChangCi");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.CcTypeId)
                .HasColumnName("CcTypeID");

            entity.Property(e => e.Etime)
                .HasColumnName("ETime")
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);

            entity.Property(e => e.Stime)
                .HasColumnName("STime")
                .HasMaxLength(8)
                .IsUnicode(false);
        }
    }
}
