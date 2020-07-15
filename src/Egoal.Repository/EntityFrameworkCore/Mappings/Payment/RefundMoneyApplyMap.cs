using Egoal.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Payment
{
    public class RefundMoneyApplyMap : IEntityTypeConfiguration<RefundMoneyApply>
    {
        public void Configure(EntityTypeBuilder<RefundMoneyApply> entity)
        {
            entity.Property(p => p.RefundListNo)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.Property(p => p.PayListNo)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            entity.Property(p => p.Reason)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.RefundRecvAccount)
                .HasMaxLength(50);

            entity.Property(p => p.RefundId)
                .HasMaxLength(50);

            entity.Property(p => p.ResultMessage)
                .HasMaxLength(200);

            entity.ToTable("OM_RefundMoneyApply");
        }
    }
}
