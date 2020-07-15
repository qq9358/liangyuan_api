using Egoal.Scenics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Scenics
{
    public class GroundDateChangCiSaleNumMap : IEntityTypeConfiguration<GroundDateChangCiSaleNum>
    {
        public void Configure(EntityTypeBuilder<GroundDateChangCiSaleNum> entity)
        {
            entity.HasKey(e => e.Id)
                     .ForSqlServerIsClustered(false);

            entity.ToTable("TM_GroundDateChangCiSaleNum");

            entity.HasIndex(e => e.Date);

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.ChangCiId)
                .HasColumnName("ChangCiID");

            entity.Property(e => e.Date)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.GroundId)
                .HasColumnName("GroundID");
        }
    }
}
