using Egoal.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Orders
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity.Property(p => p.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedNever()
                .HasColumnName("ListNo");

            entity.Property(e => e.ArrivalDate)
                    .HasMaxLength(19)
                    .IsUnicode(false);

            entity.Property(e => e.ArrivalTime)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Bid)
                .HasColumnName("BID");

            entity.Property(e => e.Cdate)
                .HasColumnName("CDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.CertNo)
                .HasMaxLength(50);

            entity.Property(e => e.CertTypeId)
                .HasColumnName("CertTypeID");

            entity.Property(e => e.CertTypeName)
                .HasMaxLength(50);

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

            entity.Property(e => e.Etime)
                .HasColumnName("ETime")
                .HasMaxLength(19)
                .IsUnicode(false);

            entity.Property(e => e.ExpressTypeId)
                .HasColumnName("ExpressTypeID");

            entity.Property(e => e.ExpressTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.GuiderId)
                .HasColumnName("GuiderID");

            entity.Property(e => e.GuiderName)
                .HasMaxLength(50);

            entity.Property(e => e.JidiaoMobile)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.JidiaoName)
                .HasMaxLength(50);

            entity.Property(e => e.KeYuanAreaId)
                .HasColumnName("KeYuanAreaID");

            entity.Property(e => e.KeYuanTypeId)
                .HasColumnName("KeYuanTypeID");

            entity.Property(e => e.LicensePlateNumber)
                .HasMaxLength(50);

            entity.Property(e => e.MemberId)
                .HasColumnName("MemberID");

            entity.Property(e => e.MemberName)
                .HasMaxLength(50);

            entity.Property(e => e.Memo)
                .HasMaxLength(200);

            entity.Property(e => e.MID)
                .HasColumnName("MID");

            entity.Property(e => e.Mobile)
                .HasMaxLength(50);

            entity.Property(e => e.MTime)
                .HasColumnName("MTime")
                .HasColumnType("datetime");

            entity.Property(e => e.OrderSmsSendTime)
                .HasColumnType("datetime");

            entity.Property(e => e.OrderStatusId)
                .HasColumnName("OrderStatusID");

            entity.Property(e => e.OrderStatusName)
                .HasMaxLength(50);

            entity.Property(e => e.OrderTypeId)
                .HasColumnName("OrderTypeID");

            entity.Property(e => e.OrderTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ParkName)
                .HasMaxLength(50);

            entity.Property(e => e.PaySmsSendTime)
                .HasColumnType("datetime");

            entity.Property(e => e.PayTime)
                .HasColumnType("datetime");

            entity.Property(e => e.PayTypeId)
                .HasColumnName("PayTypeID");

            entity.Property(e => e.PayTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.Pwd)
                .HasMaxLength(50);

            entity.Property(e => e.QuPiaoTypeId)
                .HasColumnName("QuPiaoTypeID");

            entity.Property(e => e.QuPiaoTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.SalesmanId)
                .HasColumnName("SalesmanID");

            entity.Property(e => e.ThirdPartyPlatformId)
                .HasColumnName("ThirdPartyPlatformID")
                .HasMaxLength(50);

            entity.Property(e => e.ThirdPartyPlatformOrderId)
                .HasColumnName("ThirdPartyPlatformOrderID")
                .HasMaxLength(50);

            entity.Property(e => e.Vid)
                .HasColumnName("VID");

            entity.Property(e => e.Vname)
                .HasColumnName("VName")
                .HasMaxLength(50);

            entity.Property(e => e.Vtime)
                .HasColumnName("VTime")
                .HasColumnType("datetime");

            entity.Property(e => e.YdrName)
                .HasMaxLength(50);

            entity.Property(e => e.SurplusNum)
                .IsConcurrencyToken();

            entity.Property(e => e.ChangCiId)
                .HasColumnName("OrderChangCiID");

            entity.HasIndex(e => e.CertNo);

            entity.HasIndex(e => e.CTime);

            entity.HasIndex(e => e.Etime);

            entity.HasIndex(e => e.JidiaoMobile)
                .HasName("IX_OM_Order_JiDiaoMobile");

            entity.HasIndex(e => e.MemberId);

            entity.HasIndex(e => e.ThirdPartyPlatformOrderId);

            entity.ToTable("OM_Order");
        }
    }
}
