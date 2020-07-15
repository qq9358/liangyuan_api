using Egoal.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Payment
{
    public class NetPayOrderMap : IEntityTypeConfiguration<NetPayOrder>
    {
        public void Configure(EntityTypeBuilder<NetPayOrder> entity)
        {
            entity.ToTable("OM_NetPayOrder");

            entity.HasIndex(e => e.Ctime)
                .HasName("Index_NetPayOrder_CTime");

            entity.HasIndex(e => e.ListNo)
                .HasName("Index_NetPayOrder_ListNo");

            entity.HasIndex(e => e.TransactionId)
                .HasName("Index_NetPayOrder_TransactionID");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.BankType)
                .HasMaxLength(50);

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.ErrorCode)
                .HasMaxLength(500);

            entity.Property(e => e.ListNo)
                .HasMaxLength(50);

            entity.Property(e => e.NetPayTypeId)
                .HasColumnName("NetPayTypeID");

            entity.Property(e => e.NetPayTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.OrderStatusId)
                .HasColumnName("OrderStatusID");

            entity.Property(e => e.OrderStatusName)
                .HasMaxLength(50);

            entity.Property(e => e.PayTime)
                .HasColumnType("datetime");

            entity.Property(e => e.Pcid)
                .HasColumnName("PCID")
                .HasMaxLength(10);

            entity.Property(e => e.SubPayTypeId)
                .HasMaxLength(50);

            entity.Property(e => e.SubTransactionId)
                .HasMaxLength(50);

            entity.Property(e => e.TransactionId)
                .HasColumnName("TransactionID")
                .HasMaxLength(50);

            entity.Property(e => e.JsApiPayArgs)
                .IsUnicode(false);

            entity.Property(e=>e.Mtime)
                .HasColumnName("MTime")
                .HasColumnType("datetime");
        }
    }
}
