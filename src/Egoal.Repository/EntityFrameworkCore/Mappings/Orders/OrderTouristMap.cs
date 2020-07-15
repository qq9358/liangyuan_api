using Egoal.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Orders
{
    public class OrderTouristMap : IEntityTypeConfiguration<OrderTourist>
    {
        public void Configure(EntityTypeBuilder<OrderTourist> entity)
        {
            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.CertNo)
                .HasMaxLength(50);

            entity.ToTable("OM_OrderTourist");

            entity.HasOne(p => p.OrderDetail)
                .WithMany(o => o.OrderTourists)
                .HasForeignKey(p => p.OrderDetailId);
        }
    }
}
