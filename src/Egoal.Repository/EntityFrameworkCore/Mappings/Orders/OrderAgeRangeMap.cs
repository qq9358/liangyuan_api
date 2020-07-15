using Egoal.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Orders
{
    public class OrderAgeRangeMap : IEntityTypeConfiguration<OrderAgeRange>
    {
        public void Configure(EntityTypeBuilder<OrderAgeRange> entity)
        {
            entity.Property(e => e.ListNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.ToTable("OM_OrderAgeRange");

            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderAgeRanges)
                .HasForeignKey(e => e.ListNo)
                .IsRequired();
        }
    }
}
