using Egoal.Trades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Trades
{
    public class PayDetailMap : IEntityTypeConfiguration<PayDetail>
    {
        public void Configure(EntityTypeBuilder<PayDetail> entity)
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

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.CurrencyId)
                .HasColumnName("CurrencyID");

            entity.Property(e => e.CurrencyName)
                .HasMaxLength(50);

            entity.Property(e => e.CurrencyRate)
                .HasColumnType("decimal(18, 4)");

            entity.Property(e => e.CustomerId)
                .HasColumnName("CustomerID");

            entity.Property(e => e.CzkCardNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CzkId)
                .HasColumnName("CzkID");

            entity.Property(e => e.CzkOwner)
                .HasMaxLength(50);

            entity.Property(e => e.CzkOwnerTel)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CzkTicketCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.GzStatusId)
                .HasColumnName("GzStatusID");

            entity.Property(e => e.ListNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.PayListNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.PayTypeId)
                .HasColumnName("PayTypeID");

            entity.Property(e => e.PayTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.ShiftFlag)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.ShiftTime)
                .HasColumnType("datetime");

            entity.Property(e => e.TradeId)
                .HasColumnName("TradeID");

            entity.ToTable("TM_PayDetail");

            entity.HasIndex(e => new { e.TradeId, e.ListNo })
                .HasName("IX_TM_PayDetail_TradeID")
                .ForSqlServerIsClustered();

            entity.HasOne(e => e.Trade)
                .WithMany(t => t.PayDetails)
                .HasForeignKey(e => e.TradeId)
                .IsRequired();
        }
    }
}
