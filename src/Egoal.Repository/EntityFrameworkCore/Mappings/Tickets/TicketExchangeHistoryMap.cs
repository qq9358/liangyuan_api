using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketExchangeHistoryMap : IEntityTypeConfiguration<TicketExchangeHistory>
    {
        public void Configure(EntityTypeBuilder<TicketExchangeHistory> entity)
        {
            entity.ToTable("TM_TicketExchangeHistory");

            entity.HasIndex(e => e.CashierId);

            entity.HasIndex(e => e.Ctime);

            entity.HasIndex(e => e.NewCardNo);

            entity.HasIndex(e => e.NewTicketId);

            entity.HasIndex(e => e.OldCardNo);

            entity.HasIndex(e => e.OrderListNo);

            entity.HasIndex(e => e.TradeId);

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.CashierId)
                .HasColumnName("CashierID");

            entity.Property(e => e.CashierName)
                .HasMaxLength(50);

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.NewCardNo)
                .HasMaxLength(50);

            entity.Property(e => e.NewTicketCode)
                .HasMaxLength(50);

            entity.Property(e => e.NewTicketId)
                .HasColumnName("NewTicketID");

            entity.Property(e => e.OldCardNo)
                .HasMaxLength(50);

            entity.Property(e => e.OldTicketCode)
                .HasMaxLength(50);

            entity.Property(e => e.OldTicketId)
                .HasColumnName("OldTicketID");

            entity.Property(e => e.OrderListNo)
                .HasMaxLength(50);

            entity.Property(e => e.SalePointId)
                .HasColumnName("SalePointID");

            entity.Property(e => e.SalePointName)
                .HasMaxLength(50);

            entity.Property(e => e.ThirdPartyPlatformName)
                .HasMaxLength(50);

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.Property(e => e.TicketTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.Tkid)
                .HasColumnName("TKID");

            entity.Property(e => e.Tkname)
                .HasColumnName("TKName")
                .HasMaxLength(20);

            entity.Property(e => e.TradeId).HasColumnName("TradeID");
        }
    }
}
