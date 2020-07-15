using Egoal.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Payment
{
    public class NetPayRefundFailMap : IEntityTypeConfiguration<NetPayRefundFail>
    {
        public void Configure(EntityTypeBuilder<NetPayRefundFail> entity)
        {
            entity.ToTable("OM_NetPayRefundFail");

            entity.HasIndex(e => e.ListNo)
                .HasName("IX_NetPayRefundFail_ListNo");

            entity.HasIndex(e => e.RefundListNo)
                .HasName("IX_NetPayRefundFail_RefundListNo");

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.AppId)
                .HasColumnName("AppID")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.AppKey)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.ListNo)
                .HasMaxLength(50);

            entity.Property(e => e.Memo)
                .HasMaxLength(100);

            entity.Property(e => e.NetPayTypeId)
                .HasColumnName("NetPayTypeID");

            entity.Property(e => e.NetPayTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.RefundFee)
                .HasMaxLength(20);

            entity.Property(e => e.RefundListNo)
                .HasMaxLength(50);

            entity.Property(e => e.RefundTime)
                .HasColumnType("datetime");

            entity.Property(e => e.StatusId)
                .HasColumnName("StatusID");

            entity.Property(e => e.StatusName)
                .HasMaxLength(50);

            entity.Property(e => e.TotalFee)
                .HasMaxLength(20);

            entity.Property(e => e.SalePointId)
                .HasColumnName("SalePointID");

            entity.Property(e => e.CashPcId)
                .HasColumnName("CashPCID");

            entity.Property(e => e.EmployeeNo)
                .HasMaxLength(50);
        }
    }
}
