using Egoal.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Staffs
{
    public class ExplainerTimeslotMap : IEntityTypeConfiguration<ExplainerTimeslot>
    {
        public void Configure(EntityTypeBuilder<ExplainerTimeslot> entity)
        {
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.BeginTime)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(5);

            entity.Property(p => p.EndTime)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(5);

            entity.ToTable("RM_ExplainerTimeslot");
        }
    }
}
