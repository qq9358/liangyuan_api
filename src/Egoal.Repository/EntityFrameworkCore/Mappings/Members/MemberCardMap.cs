using Egoal.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Members
{
    public class MemberCardMap : IEntityTypeConfiguration<MemberCard>
    {
        public void Configure(EntityTypeBuilder<MemberCard> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.AviateFlag)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.CardNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CardValidFlagName)
                .HasMaxLength(50);

            entity.Property(e => e.CID);

            entity.Property(e => e.CTime)
                .HasColumnType("datetime");

            entity.Property(e => e.Etime)
                .HasColumnName("ETime")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.ListNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.MemberAccountId)
                .HasColumnName("MemberAccountID");

            entity.Property(e => e.MemberId)
                .HasColumnName("MemberID");

            entity.Property(e => e.PrincipalCard)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.Stime)
                .HasColumnName("STime")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.TicketCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.TicketId)
                .HasColumnName("TicketID");

            entity.Property(e => e.TicketStatusId)
                .HasColumnName("TicketStatusID");

            entity.Property(e => e.TicketStatusName)
                .HasMaxLength(50);

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.Property(e => e.TicketTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.TradeId)
                .HasColumnName("TradeID");

            entity.HasIndex(e => e.CardNo);

            entity.HasIndex(e => e.MemberAccountId);

            entity.HasIndex(e => e.TicketCode);

            entity.ToTable("MM_MemberCard");
        }
    }
}
