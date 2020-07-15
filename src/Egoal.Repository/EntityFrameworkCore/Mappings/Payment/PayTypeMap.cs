using Egoal.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Payment
{
    public class PayTypeMap : IEntityTypeConfiguration<PayType>
    {
        public void Configure(EntityTypeBuilder<PayType> entity)
        {
            entity.ToTable("SM_PayType");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);

            entity.Property(e => e.Supplier)
                .HasMaxLength(50);

            entity.Property(e => e.ServicePhone)
                .IsUnicode(false)
                .HasMaxLength(20);
        }
    }
}
