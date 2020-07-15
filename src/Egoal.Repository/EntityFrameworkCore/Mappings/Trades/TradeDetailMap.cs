using Egoal.Trades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Trades
{
    public class TradeDetailMap : IEntityTypeConfiguration<TradeDetail>
    {
        public void Configure(EntityTypeBuilder<TradeDetail> entity)
        {
            entity.HasKey(e => e.Id)
                .ForSqlServerIsClustered(false);

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.Bid)
                .HasColumnName("BID");

            entity.Property(e => e.CashierId)
                .HasColumnName("CashierID");

            entity.Property(e => e.Cdate)
                .HasColumnName("CDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Cmonth)
                .HasColumnName("CMonth")
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.Property(e => e.Cquarter)
                .HasColumnName("CQuarter")
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Ctp)
                .HasColumnName("CTP")
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.Property(e => e.CurrencyId)
                .HasColumnName("CurrencyID");

            entity.Property(e => e.CurrencyName)
                .HasMaxLength(50);

            entity.Property(e => e.CurrencyRate)
                .HasColumnType("decimal(18, 4)");

            entity.Property(e => e.Cweek)
                .HasColumnName("CWeek")
                .HasMaxLength(3);

            entity.Property(e => e.Cyear)
                .HasColumnName("CYear")
                .HasMaxLength(4)
                .IsUnicode(false);

            entity.Property(e => e.InvoiceCode)
                .HasMaxLength(50);

            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(50);

            entity.Property(e => e.ListNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ShiftFlag)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.ShiftTime)
                .HasColumnType("datetime");

            entity.Property(e => e.TradeId)
                .HasColumnName("TradeID");

            entity.Property(e => e.TradeTypeId)
                .HasColumnName("TradeTypeID");

            entity.Property(e => e.TradeTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.TradeTypeTypeId)
                .HasColumnName("TradeTypeTypeID");

            entity.ToTable("TM_TradeDetail");

            entity.HasIndex(e => new { e.TradeId, e.ListNo })
                .HasName("IX_TM_TradeDetail_TradeID")
                .ForSqlServerIsClustered();

            entity.HasOne(e => e.Trade)
                .WithMany(t => t.TradeDetails)
                .HasForeignKey(e => e.TradeId)
                .IsRequired();
        }
    }
}
