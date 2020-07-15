using Egoal.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Customers
{
    public class CustomerTypeMap : IEntityTypeConfiguration<CustomerType>
    {
        public void Configure(EntityTypeBuilder<CustomerType> entity)
        {
            entity.ToTable("CM_CustomerType");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);
        }
    }
}
