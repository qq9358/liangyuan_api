using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketReprintLogMap : IEntityTypeConfiguration<TicketReprintLog>
    {
        public void Configure(EntityTypeBuilder<TicketReprintLog> entity)
        {
            entity.ToTable("TM_TicketReprintLog");

            entity.HasIndex(e => e.CardNo)
                .HasName("IX_TicketReprintLog_CardNo");

            entity.HasIndex(e => e.Ctime)
                .HasName("IX_TicketReprintLog_CTime");

            entity.HasIndex(e => e.TicketCode)
                .HasName("IX_TicketReprintLog_TicketCode");

            entity.HasIndex(e => e.TicketTypeId)
                .HasName("IX_TicketReprintLog_TicketTypeID");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.CardNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CashPcid)
                .HasColumnName("CashPCID");

            entity.Property(e => e.CashPcname)
                .HasColumnName("CashPCName")
                .HasMaxLength(50);

            entity.Property(e => e.CashierId)
                .HasColumnName("CashierID");

            entity.Property(e => e.CashierName)
                .HasMaxLength(50);

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ParkName)
                .HasMaxLength(50);

            entity.Property(e => e.SalePointId)
                .HasColumnName("SalePointID");

            entity.Property(e => e.TicketCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.TicketId)
                .HasColumnName("TicketID");

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.Property(e => e.TicketTypeName)
                .HasMaxLength(50);
        }
    }
}
