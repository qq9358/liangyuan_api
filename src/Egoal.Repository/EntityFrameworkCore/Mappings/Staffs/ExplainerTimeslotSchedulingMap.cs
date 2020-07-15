using Egoal.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Staffs
{
    public class ExplainerTimeslotSchedulingMap : IEntityTypeConfiguration<ExplainerTimeslotScheduling>
    {
        public void Configure(EntityTypeBuilder<ExplainerTimeslotScheduling> entity)
        {
            entity.Property(p => p.Date)
                .HasMaxLength(10)
                .IsRequired()
                .IsUnicode(false);

            entity.ToTable("RM_ExplainerTimeslotScheduling");
        }
    }
}
