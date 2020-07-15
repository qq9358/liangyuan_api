using Egoal.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Orders
{
    public class RefundOrderApplyMap : IEntityTypeConfiguration<RefundOrderApply>
    {
        public void Configure(EntityTypeBuilder<RefundOrderApply> entity)
        {
            entity.Property(p => p.RefundListNo)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.Property(p => p.ListNo)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.Property(p => p.Details)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(p => p.Reason)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.ResultMessage)
                .HasMaxLength(100);

            entity.ToTable("OM_RefundOrderApply");
        }
    }
}
