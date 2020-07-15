using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketSaleBuyerMap : IEntityTypeConfiguration<TicketSaleBuyer>
    {
        public void Configure(EntityTypeBuilder<TicketSaleBuyer> entity)
        {
            entity.ToTable("TM_TicketSaleBuyer");

            entity.HasIndex(e => e.CertNo);

            entity.HasIndex(e => e.Ctime);

            entity.HasIndex(e => e.OrderListNo);

            entity.HasIndex(e => e.Sdate);

            entity.HasIndex(e => e.TicketId);

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Address)
                .HasMaxLength(100);

            entity.Property(e => e.Area)
                .HasMaxLength(50);

            entity.Property(e => e.Birthday)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.BuyerName)
                .HasMaxLength(50);

            entity.Property(e => e.CertBlackListHandleFlag)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.CertEdate)
                .HasColumnName("CertEDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.CertNo)
                .HasMaxLength(50);

            entity.Property(e => e.CertSdate)
                .HasColumnName("CertSDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.CertTypeId)
                .HasColumnName("CertTypeID");

            entity.Property(e => e.CertTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.ChinaCityId)
                .HasColumnName("ChinaCityID")
                .HasMaxLength(20);

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Department)
                .HasMaxLength(100);

            entity.Property(e => e.Etime)
                .HasColumnName("ETime")
                .HasColumnType("datetime");

            entity.Property(e => e.Memo)
                .HasMaxLength(200);

            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Nation)
                .HasMaxLength(50);

            entity.Property(e => e.OrderListNo)
                .HasMaxLength(50);

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ProvinceId)
                .HasColumnName("ProvinceID");

            entity.Property(e => e.Sdate)
                .HasColumnName("SDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Sex)
                .HasMaxLength(1);

            entity.Property(e => e.Stime)
                .HasColumnName("STime")
                .HasColumnType("datetime");

            entity.Property(e => e.TicketId)
                .HasColumnName("TicketID");

            entity.Property(e => e.TradeId)
                .HasColumnName("TradeID");

            entity.HasOne(e => e.TicketSale)
                .WithMany(t => t.TicketSaleBuyers)
                .HasForeignKey(e => e.TicketId);
        }
    }
}
