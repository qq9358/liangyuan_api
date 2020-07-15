using Egoal.Stadiums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Stadiums
{
    public class SeatMap : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.ToTable("SS_Seat");

            entity.HasIndex(e => new { e.StadiumId, e.RegionId, e.X, e.Y })
                .HasName("IX_SS_Seat")
                .IsUnique()
                .ForSqlServerIsClustered();

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Bid)
                .HasColumnName("BID");

            entity.Property(e => e.Code)
                .HasMaxLength(50);

            entity.Property(e => e.GroupCode)
                .HasMaxLength(50);

            entity.Property(e => e.Lbid)
                .HasColumnName("LBID");

            entity.Property(e => e.Lbid2)
                .HasColumnName("LBID2");

            entity.Property(e => e.LockTime)
                .HasColumnType("datetime");

            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.Property(e => e.RegionId)
                .HasColumnName("RegionID");

            entity.Property(e => e.SeatTypeId)
                .HasColumnName("SeatTypeID");

            entity.Property(e => e.StadiumId)
                .HasColumnName("StadiumID");

            entity.Property(e => e.StadiumTypeId)
                .HasColumnName("StadiumTypeID");

            entity.Property(e => e.Xlen)
                .HasColumnName("XLen");

            entity.Property(e => e.Ylen)
                .HasColumnName("YLen");
        }
    }
}
