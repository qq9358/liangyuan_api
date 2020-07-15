using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketSaleDayStatMap : IEntityTypeConfiguration<TicketSaleDayStat>
    {
        public void Configure(EntityTypeBuilder<TicketSaleDayStat> entity)
        {
            entity.ToTable("TM_TicketSaleDayStat");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.CashPcid)
                .HasColumnName("CashPCID");

            entity.Property(e => e.CashierId)
                .HasColumnName("CashierID");

            entity.Property(e => e.Cdate)
                .HasColumnName("CDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Ctp)
                .HasColumnName("CTP")
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");
        }
    }
}
