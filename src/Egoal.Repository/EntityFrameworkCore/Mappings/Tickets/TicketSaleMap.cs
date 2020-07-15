using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketSaleMap : IEntityTypeConfiguration<TicketSale>
    {
        public void Configure(EntityTypeBuilder<TicketSale> entity)
        {
            entity.HasKey(e => e.Id)
                .ForSqlServerIsClustered(false);

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.AreaId)
                .HasColumnName("AreaID");

            entity.Property(e => e.AreaName)
                .HasMaxLength(50);

            entity.Property(e => e.AuthorizedTicketId)
                .HasColumnName("AuthorizedTicketID");

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

            entity.Property(e => e.CertNo)
                .HasMaxLength(50);

            entity.Property(e => e.CertTypeId)
                .HasColumnName("CertTypeID");

            entity.Property(e => e.CertTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.ChangCiId)
                .HasColumnName("ChangCiID");

            entity.Property(e => e.CheckRule)
                .IsUnicode(false);

            entity.Property(e => e.CheckTypeId)
                .HasColumnName("CheckTypeID");

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

            entity.Property(e => e.DiscountApproverId)
                .HasColumnName("DiscountApproverID");

            entity.Property(e => e.DiscountApproverName)
                .HasMaxLength(50);

            entity.Property(e => e.DiscountTypeId)
                .HasColumnName("DiscountTypeID");

            entity.Property(e => e.DiscountTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.Etime)
                .HasColumnName("ETime")
                .HasMaxLength(19)
                .IsUnicode(false);

            entity.Property(e => e.FingerStatusId)
                .HasColumnName("FingerStatusID");

            entity.Property(e => e.GroundId)
                .HasColumnName("GroundID");

            entity.Property(e => e.GuiderId)
                .HasColumnName("GuiderID");

            entity.Property(e => e.GuiderName)
                .HasMaxLength(50);

            entity.Property(e => e.GuiderSharingMoney)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.InvoiceCode)
                .HasMaxLength(50);

            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(50);

            entity.Property(e => e.IsRechargeFlag)
                .HasDefaultValueSql("((0))");

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

            entity.Property(e => e.OrderDetailId)
                .HasColumnName("OrderDetailID");

            entity.Property(e => e.OrderListNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ParkName)
                .HasMaxLength(50);

            entity.Property(e => e.PayTypeId)
                .HasColumnName("PayTypeID");

            entity.Property(e => e.PayTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.PermitStaffId)
                .HasColumnName("PermitStaffID");

            entity.Property(e => e.PriceTypeId)
                .HasColumnName("PriceTypeID");

            entity.Property(e => e.PriceTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.ReturnApproverId)
                .HasColumnName("ReturnApproverID");

            entity.Property(e => e.ReturnApproverName)
                .HasMaxLength(50);

            entity.Property(e => e.ReturnTicketId)
                .HasColumnName("ReturnTicketID");

            entity.Property(e => e.ReturnTypeId)
                .HasColumnName("ReturnTypeID");

            entity.Property(e => e.ReturnTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.SalePointId)
                .HasColumnName("SalePointID");

            entity.Property(e => e.SalePointName)
                .HasMaxLength(50);

            entity.Property(e => e.SalesmanId)
                .HasColumnName("SalesmanID");

            entity.Property(e => e.SalesmanName)
                .HasMaxLength(50);

            entity.Property(e => e.Sdate)
                .HasColumnName("SDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.SettleTime)
                .HasColumnType("datetime");

            entity.Property(e => e.ShiftFlag)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.ShiftTime)
                .HasColumnType("datetime");

            entity.Property(e => e.Stime)
                .HasColumnName("STime")
                .HasMaxLength(19)
                .IsUnicode(false);

            entity.Property(e => e.TdCode)
                .HasMaxLength(500);

            entity.Property(e => e.TheTicketDate)
                .HasMaxLength(50);

            entity.Property(e => e.TicketBindTypeId)
                .HasColumnName("TicketBindTypeID");

            entity.Property(e => e.TicketCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.TicketStatusId)
                .HasColumnName("TicketStatusID");

            entity.Property(e => e.TicketStatusName)
                .HasMaxLength(50);

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.Property(e => e.TicketTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.TicketTypeProjectId)
                .HasColumnName("TicketTypeProjectID");

            entity.Property(e => e.TicketTypeProjectTypeId)
                .HasColumnName("TicketTypeProjectTypeID");

            entity.Property(e => e.TicketTypeTypeId)
                .HasColumnName("TicketTypeTypeID");

            entity.Property(e => e.Tkid)
                .HasColumnName("TKID");

            entity.Property(e => e.Tkname)
                .HasColumnName("TKName")
                .HasMaxLength(50);

            entity.Property(e => e.TradeId)
                .HasColumnName("TradeID");

            entity.Property(e => e.Ttid)
                .HasColumnName("TTID");

            entity.Property(e => e.ValidFlagName)
                .HasMaxLength(50);

            entity.Property(e => e.YsqTicketCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.SurplusNum)
                .IsConcurrencyToken();

            entity.ToTable("TM_TicketSale");

            entity.HasIndex(e => e.CardNo);

            entity.HasIndex(e => e.CertNo);

            entity.HasIndex(e => e.Ctime)
                .ForSqlServerIsClustered();

            entity.HasIndex(e => e.Etime);

            entity.HasIndex(e => e.ListNo);

            entity.HasIndex(e => e.OrderListNo);

            entity.HasIndex(e => e.Sdate);

            entity.HasIndex(e => e.SyncCode);

            entity.HasIndex(e => e.TicketCode);

            entity.HasIndex(e => e.TradeId);

            entity.Ignore(e => e.TicketType);
        }
    }
}
