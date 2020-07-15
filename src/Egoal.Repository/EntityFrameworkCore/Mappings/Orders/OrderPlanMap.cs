using Egoal.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Orders
{
    public class OrderPlanMap : IEntityTypeConfiguration<OrderPlan>
    {
        public void Configure(EntityTypeBuilder<OrderPlan> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Date)
                .HasColumnType("datetime");

            entity.Property(e => e.Week)
                .IsRequired()
                .HasMaxLength(10);

            entity.ToTable("OM_OrderPlan");
        }
    }
}
