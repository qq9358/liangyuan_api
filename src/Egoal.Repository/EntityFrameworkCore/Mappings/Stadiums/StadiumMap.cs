using Egoal.Stadiums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Stadiums
{
    public class StadiumMap : IEntityTypeConfiguration<Stadium>
    {
        public void Configure(EntityTypeBuilder<Stadium> entity)
        {
            entity.ToTable("SS_Stadium");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Memo)
                .HasMaxLength(100);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.SeatCodePrefix)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.SeatTypeId)
                .HasColumnName("SeatTypeID");

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);

            entity.Property(e => e.StadiumTypeId)
                .HasColumnName("StadiumTypeID");

            entity.Property(e => e.Xcount)
                .HasColumnName("XCount");

            entity.Property(e => e.Ycount)
                .HasColumnName("YCount");
        }
    }
}
