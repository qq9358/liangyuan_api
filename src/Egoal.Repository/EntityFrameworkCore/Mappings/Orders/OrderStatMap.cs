using Egoal.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Orders
{
    public class OrderStatMap : IEntityTypeConfiguration<OrderStat>
    {
        public void Configure(EntityTypeBuilder<OrderStat> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Cdate)
                .IsRequired()
                .HasColumnName("CDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.ToTable("OM_OrderStat");
        }
    }
}
