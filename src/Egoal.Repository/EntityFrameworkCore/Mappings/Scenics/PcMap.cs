using Egoal.Scenics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Scenics
{
    public class PcMap : IEntityTypeConfiguration<Pc>
    {
        public void Configure(EntityTypeBuilder<Pc> entity)
        {
            entity.ToTable("RM_PC");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Cid)
                .HasColumnName("CID");

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Ip)
                .HasColumnName("IP")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Mac)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Memo)
                .HasMaxLength(100);

            entity.Property(e => e.Mid)
                .HasColumnName("MID");

            entity.Property(e => e.Mtime)
                .HasColumnName("MTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.SalePointId)
                .HasColumnName("SalePointID");

            entity.Property(e => e.WareShopId)
                .HasColumnName("WareShopID");

            entity.Property(e => e.WareShopName)
                .HasMaxLength(50);

            entity.Property(e => e.IdentifyCode)
                .HasMaxLength(50);
        }
    }
}
