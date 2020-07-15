using Egoal.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Orders
{
    public class OrderGroundChangCiMap : IEntityTypeConfiguration<OrderGroundChangCi>
    {
        public void Configure(EntityTypeBuilder<OrderGroundChangCi> entity)
        {
            entity.ToTable("OM_OrderGroundChangCi");

            entity.HasOne(p => p.OrderDetail)
                .WithMany(o => o.OrderGroundChangCis)
                .HasForeignKey(p => p.OrderDetailId);
        }
    }
}
