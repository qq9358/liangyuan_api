using Egoal.Scenics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Scenics
{
    public class GroundChangCiPlanMap : IEntityTypeConfiguration<GroundChangCiPlan>
    {
        public void Configure(EntityTypeBuilder<GroundChangCiPlan> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.ToTable("TM_WeekChangCiPlan");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.ChangCiId)
                .HasColumnName("ChangCiID");

            entity.Property(e => e.GroundId)
                .HasColumnName("GroundID");

            entity.Property(e => e.Week)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}
