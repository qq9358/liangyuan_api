using Egoal.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Customers
{
    public class CustomerPhotoMap : IEntityTypeConfiguration<CustomerPhoto>
    {
        public void Configure(EntityTypeBuilder<CustomerPhoto> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.CustomerId)
                .HasColumnName("CustomerID");

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ParkName)
                .HasMaxLength(50);

            entity.ToTable("CM_CustomerPhoto");

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.CustomerPhotos)
                .HasForeignKey(e => e.CustomerId);
        }
    }
}
