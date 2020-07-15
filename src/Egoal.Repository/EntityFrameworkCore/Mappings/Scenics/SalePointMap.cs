using Egoal.Scenics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Scenics
{
    public class SalePointMap : IEntityTypeConfiguration<SalePoint>
    {
        public void Configure(EntityTypeBuilder<SalePoint> entity)
        {
            entity.ToTable("RM_SalePoint");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.SalePointType)
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);
        }
    }
}
