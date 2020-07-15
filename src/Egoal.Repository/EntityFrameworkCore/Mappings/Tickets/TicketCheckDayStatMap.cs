using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketCheckDayStatMap : IEntityTypeConfiguration<TicketCheckDayStat>
    {
        public void Configure(EntityTypeBuilder<TicketCheckDayStat> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Cdate)
                .HasColumnName("CDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.CheckerId)
                .HasColumnName("CheckerID");

            entity.Property(e => e.Ctp)
                .HasColumnName("CTP")
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.Property(e => e.GateGroupId)
                .HasColumnName("GateGroupID");

            entity.Property(e => e.GateId)
                .HasColumnName("GateID");

            entity.Property(e => e.GroundId)
                .HasColumnName("GroundID");

            entity.ToTable("TM_TicketCheckDayStat");

            entity.HasIndex(e => e.Cdate)
                .HasName("IX_TicketCheckDayStat_CDate");
        }
    }
}
