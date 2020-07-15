using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketConsumeMap : IEntityTypeConfiguration<TicketConsume>
    {
        public void Configure(EntityTypeBuilder<TicketConsume> entity)
        {
            entity.Property(e => e.CardNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CertNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ConsumeTime)
                .HasColumnType("datetime");

            entity.Property(e => e.LastNoticeTime)
                .HasColumnType("datetime");

            entity.Property(e => e.ThirdPartyPlatformId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ThirdPartyPlatformOrderId)
                .HasColumnName("ThirdPartyPlatformOrderID")
                .HasMaxLength(50);

            entity.Property(e => e.TicketTypeName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TradeId)
                .HasColumnName("TradeID");

            entity.ToTable("TM_TicketConsume");

            entity.HasIndex(e => e.CardNo)
                .HasName("IX_TicketConsume_CardNo");

            entity.HasIndex(e => e.ConsumeTime)
                .HasName("IX_TicketConsume_ConsumeTime");

            entity.HasIndex(e => e.TicketConsumeGuid)
                .HasName("IX_TicketConsume_TicketConsumeGuid");

            entity.HasIndex(e => e.TicketId)
                .HasName("IX_TicketConsume_TicketID");

            entity.HasIndex(e => e.TradeId)
                .HasName("IX_TicketConsume_TradeID");
        }
    }
}
