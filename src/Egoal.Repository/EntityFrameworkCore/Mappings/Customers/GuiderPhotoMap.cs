using Egoal.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Customers
{
    public class GuiderPhotoMap : IEntityTypeConfiguration<GuiderPhoto>
    {
        public void Configure(EntityTypeBuilder<GuiderPhoto> entity)
        {
            entity.HasKey(e => e.Id)
                .ForSqlServerIsClustered(false);

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.GuiderId)
                .HasColumnName("GuiderID");

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ParkName)
                .HasMaxLength(50);

            entity.ToTable("CM_GuiderPhoto");
        }
    }
}
