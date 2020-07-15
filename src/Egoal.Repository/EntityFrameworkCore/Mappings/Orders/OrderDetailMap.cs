using Egoal.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Orders
{
    public class OrderDetailMap : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Bid)
                .HasColumnName("BID");

            entity.Property(e => e.Cdate)
                .HasColumnName("CDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.CertBlackListHandleFlag)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.CertNo)
                .HasMaxLength(50);

            entity.Property(e => e.ChangCiId)
                .HasColumnName("ChangCiID");

            entity.Property(e => e.CheckRule)
                .IsUnicode(false);

            entity.Property(e => e.CID)
                .HasColumnName("CID");

            entity.Property(e => e.Cmonth)
                .HasColumnName("CMonth")
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.Property(e => e.Cquarter)
                .HasColumnName("CQuarter")
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.Property(e => e.CTime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Ctp)
                .HasColumnName("CTP")
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.Property(e => e.CustomerId)
                .HasColumnName("CustomerID");

            entity.Property(e => e.CustomerName)
                .HasMaxLength(50);

            entity.Property(e => e.Cweek)
                .HasColumnName("CWeek")
                .HasMaxLength(3);

            entity.Property(e => e.Cyear)
                .HasColumnName("CYear")
                .HasMaxLength(4)
                .IsUnicode(false);

            entity.Property(e => e.DiscountTypeId)
                .HasColumnName("DiscountTypeID");

            entity.Property(e => e.DiscountTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.Etime)
                .HasColumnName("ETime")
                .HasColumnType("datetime");

            entity.Property(e => e.GuiderId)
                .HasColumnName("GuiderID");

            entity.Property(e => e.GuiderName)
                .HasMaxLength(50);

            entity.Property(e => e.ListNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.MemberId)
                .HasColumnName("MemberID");

            entity.Property(e => e.MemberName)
                .HasMaxLength(50);

            entity.Property(e => e.Memo)
                .HasMaxLength(200);

            entity.Property(e => e.MID)
                .HasColumnName("MID");

            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.MTime)
                .HasColumnName("MTime")
                .HasColumnType("datetime");

            entity.Property(e => e.OrderTypeId)
                .HasColumnName("OrderTypeID");

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ParkName)
                .HasMaxLength(50);

            entity.Property(e => e.Stime)
                .HasColumnName("STime")
                .HasColumnType("datetime");

            entity.Property(e => e.TicketCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.TicketEtime)
                .HasColumnName("TicketETime")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.TicketStime)
                .HasColumnName("TicketSTime")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.Property(e => e.TicketTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.TouristName)
                .HasMaxLength(50);

            entity.Property(e => e.WeekendUse)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.SurplusNum)
                .IsConcurrencyToken();

            entity.ToTable("OM_OrderDetail");

            entity.HasIndex(e => e.CertNo);

            entity.HasIndex(e => e.CTime)
                .ForSqlServerIsClustered();

            entity.HasIndex(e => e.Etime);

            entity.HasIndex(e => e.ListNo);

            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(e => e.ListNo)
                .IsRequired();

            entity.Ignore(e => e.TicketType);
            entity.Ignore(e => e.Tourist);
        }
    }
}
