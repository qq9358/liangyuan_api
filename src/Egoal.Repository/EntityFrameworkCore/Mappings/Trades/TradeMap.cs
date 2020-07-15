using Egoal.Trades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Trades
{
    public class TradeMap : IEntityTypeConfiguration<Trade>
    {
        public void Configure(EntityTypeBuilder<Trade> entity)
        {
            entity.HasKey(e => e.Id)
                .ForSqlServerIsClustered(false);

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.ApproverId)
                .HasColumnName("ApproverID");

            entity.Property(e => e.ApproverName)
                .HasMaxLength(50);

            entity.Property(e => e.AreaId)
                .HasColumnName("AreaID");

            entity.Property(e => e.AreaName)
                .HasMaxLength(50);

            entity.Property(e => e.Bid)
                .HasColumnName("BID");

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

            entity.Property(e => e.Cdate)
                .HasColumnName("CDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Cmonth)
                .HasColumnName("CMonth")
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.Property(e => e.Cquarter)
                .HasColumnName("CQuarter")
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Ctp)
                .HasColumnName("CTP")
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.Property(e => e.CurrencyId)
                .HasColumnName("CurrencyID");

            entity.Property(e => e.CurrencyName)
                .HasMaxLength(50);

            entity.Property(e => e.CurrencyRate)
                .HasColumnType("decimal(18, 4)");

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

            entity.Property(e => e.GuiderId)
                .HasColumnName("GuiderID");

            entity.Property(e => e.GuiderName)
                .HasMaxLength(50);

            entity.Property(e => e.InvoiceCode)
                .HasMaxLength(50);

            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(50);

            entity.Property(e => e.KeYuanTypeId)
                .HasColumnName("KeYuanTypeID");

            entity.Property(e => e.KeYuanTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.ListNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Ltime)
                .HasColumnName("LTime")
                .HasColumnType("datetime");

            entity.Property(e => e.ManagerId)
                .HasColumnName("ManagerID");

            entity.Property(e => e.ManagerName)
                .HasMaxLength(50);

            entity.Property(e => e.MemberId)
                .HasColumnName("MemberID");

            entity.Property(e => e.MemberName)
                .HasMaxLength(50);

            entity.Property(e => e.Memo)
                .HasMaxLength(100);

            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.OrderListNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Ota)
                .HasColumnName("OTA")
                .HasMaxLength(50);

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ParkName)
                .HasMaxLength(50);

            entity.Property(e => e.PayTypeId)
                .HasColumnName("PayTypeID");

            entity.Property(e => e.PayTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.SalePointId)
                .HasColumnName("SalePointID");

            entity.Property(e => e.SalePointName)
                .HasMaxLength(50);

            entity.Property(e => e.SalesmanId)
                .HasColumnName("SalesmanID");

            entity.Property(e => e.SalesmanName)
                .HasMaxLength(50);

            entity.Property(e => e.ShiftFlag)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.ShiftTime)
                .HasColumnType("datetime");

            entity.Property(e => e.ShopId)
                .HasColumnName("ShopID");

            entity.Property(e => e.ShopName)
                .HasMaxLength(50);

            entity.Property(e => e.ThirdPartyPlatformId)
                .HasColumnName("ThirdPartyPlatformID")
                .HasMaxLength(50);

            entity.Property(e => e.ThirdPartyPlatformOrderId)
                .HasColumnName("ThirdPartyPlatformOrderID")
                .HasMaxLength(50);

            entity.Property(e => e.TicketId)
                .HasColumnName("TicketID");

            entity.Property(e => e.TradeSource)
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.TradeTypeId)
                .HasColumnName("TradeTypeID");

            entity.Property(e => e.TradeTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.TradeTypeTypeId)
                .HasColumnName("TradeTypeTypeID");

            entity.ToTable("TM_Trade");

            entity.HasIndex(e => e.Ctime)
                .ForSqlServerIsClustered();

            entity.HasIndex(e => e.InvoiceNo)
                .HasName("IX_TM_Trade_Invoice");

            entity.HasIndex(e => e.ListNo);

            entity.HasIndex(e => e.ThirdPartyPlatformOrderId);
        }
    }
}
