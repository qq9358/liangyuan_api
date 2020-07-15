using Egoal.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Common
{
    public class TmDateMap : IEntityTypeConfiguration<TmDate>
    {
        public void Configure(EntityTypeBuilder<TmDate> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.ToTable("TM_Date");

            entity.HasIndex(e => new { e.Date, e.ParkId })
                .HasName("IX_TM_Date")
                .IsUnique()
                .ForSqlServerIsClustered();

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Date)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.DateTypeId)
                .HasColumnName("DateTypeID");

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.PriceTypeId)
                .HasColumnName("PriceTypeID");
        }
    }
}
